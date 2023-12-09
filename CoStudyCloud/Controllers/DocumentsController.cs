using CoStudyCloud.Core.ViewModels;
using CoStudyCloud.Infrastructure.CloudStorage;
using CoStudyCloud.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CoStudyCloud.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly ICloudStorage _cloudStorage;

        public DocumentsController(ICloudStorage cloudStorage)
        {
            _cloudStorage = cloudStorage;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(DocumentFormViewModel documentFormViewModel)
        {
            if (!ModelState.IsValid || documentFormViewModel.DocumentFile == null)
            {
                return View();
            }

            await UploadFile(documentFormViewModel);

            //TODO: Add a message to show file upload succeeded

            return View();
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
