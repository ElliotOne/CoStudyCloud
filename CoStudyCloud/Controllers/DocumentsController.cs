using AutoMapper;
using CoStudyCloud.Core.Models;
using CoStudyCloud.Core.Repositories;
using CoStudyCloud.Core.ViewModels;
using CoStudyCloud.Infrastructure.CloudStorage;
using CoStudyCloud.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace CoStudyCloud.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IStudyGroupRepository _studyGroupRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICloudStorage _cloudStorage;
        private readonly IMapper _mapper;

        public DocumentsController(
            IDocumentRepository documentRepository,
            IStudyGroupRepository studyGroupRepository,
            IUserRepository userRepository,
            ICloudStorage cloudStorage,
            IMapper mapper)
        {
            _documentRepository = documentRepository;
            _studyGroupRepository = studyGroupRepository;
            _userRepository = userRepository;
            _cloudStorage = cloudStorage;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value!;

            var user = await _userRepository.GetByEmail(email);

            if (user == null)
            {
                return BadRequest();
            }

            var documentsWithOwnerStatus =
                await _documentRepository.GetDocumentsWithOwnerStatus(user.Id!);

            var documentIndexViewModel = new DocumentIndexViewModel()
            {
                DocumentWithOwnerStatusViewModels = _mapper.Map<
                IEnumerable<DocumentWithOwnerStatus>, IEnumerable<DocumentWithOwnerStatusViewModel>>(documentsWithOwnerStatus)
            };

            return View(documentIndexViewModel);
        }

        public async Task<IActionResult> Upload()
        {
            ViewData[nameof(DocumentFormViewModel.StudyGroupId)] = await GetStudyGroups();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(DocumentFormViewModel documentFormViewModel)
        {
            if (!ModelState.IsValid || documentFormViewModel.DocumentFile == null)
            {
                ViewData[nameof(DocumentFormViewModel.StudyGroupId)] = await GetStudyGroups();

                return View(documentFormViewModel);
            }

            await UploadFile(documentFormViewModel);

            var document = _mapper.Map<DocumentFormViewModel, Document>(documentFormViewModel);

            var email = User.FindFirst(ClaimTypes.Email)?.Value!;

            var user = await _userRepository.GetByEmail(email);

            if (user == null)
            {
                return BadRequest();
            }

            document.UploaderUserId = user.Id;
            document.CreateDate = DateTime.UtcNow;

            await _documentRepository.Add(document);

            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        public async Task<SelectList> GetStudyGroups()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value!;

            var user = await _userRepository.GetByEmail(email);

            if (user == null)
            {
                return new SelectList(null);
            }

            var studyGroups =
                await _studyGroupRepository.GetAllStudyGroupsByUserId(user.Id!);

            return new SelectList(
                studyGroups,
                nameof(StudyGroup.Id),
                nameof(StudyGroup.Title));
        }

        private async Task UploadFile(DocumentFormViewModel documentFormViewModel)
        {
            if (documentFormViewModel.DocumentFile?.FileName != null)
            {
                documentFormViewModel.FileName = GenerateUniqFileName(documentFormViewModel.DocumentFile.FileName);
                documentFormViewModel.FileUrl =
                    await _cloudStorage.UploadFileAsync(documentFormViewModel.DocumentFile, documentFormViewModel.FileName);
            }
        }

        private static string GenerateUniqFileName(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

            // Replace spaces with underscores in the file name
            string urlFriendlyFileName = fileNameWithoutExtension.Replace(" ", "_");

            return $"{urlFriendlyFileName}_{GuidUtility.GenerateUrlFriendlyGuid()}{fileExtension}";
        }
    }
}
