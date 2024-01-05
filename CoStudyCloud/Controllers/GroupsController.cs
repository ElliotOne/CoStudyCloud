using AutoMapper;
using CoStudyCloud.Core.Models;
using CoStudyCloud.Core.Repositories;
using CoStudyCloud.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoStudyCloud.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        private readonly IStudyGroupRepository _studyGroupRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GroupsController(
            IStudyGroupRepository studyGroupRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _studyGroupRepository = studyGroupRepository;
            _userRepository = userRepository;
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

            var groupsWithJoinStatus =
                await _studyGroupRepository.GetStudyGroupsWithJoinStatus(user.Id!);

            var groupIndexViewModel = new GroupIndexViewModel()
            {
                StudyGroupWithJoinStatusViewModels = _mapper.Map<
                    IEnumerable<StudyGroupWithJoinStatus>, IEnumerable<StudyGroupWithJoinStatusViewModel>>(
                    groupsWithJoinStatus)
            };

            return View(groupIndexViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Join(string groupId)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value!;

            var user = await _userRepository.GetByEmail(email);

            if (user == null)
            {
                return Json(BadRequest());
            }

            var userStudyGroup = new UserStudyGroup()
            {
                UserId = user.Id,
                StudyGroupId = groupId
            };

            await _studyGroupRepository.AddUserToStudyGroup(userStudyGroup);

            return Json(Ok());
        }

        [HttpPost]
        public async Task<IActionResult> Leave(string mappingId)
        {
            await _studyGroupRepository.RemoveUserFromStudyGroup(mappingId);

            return Json(Ok());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GroupFormViewModel groupFormViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(groupFormViewModel);
            }

            var studyGroup = _mapper.Map<GroupFormViewModel, StudyGroup>(groupFormViewModel);

            var email = User.FindFirst(ClaimTypes.Email)?.Value!;

            var user = await _userRepository.GetByEmail(email);

            if (user == null)
            {
                return BadRequest();
            }

            studyGroup.AdminUserId = user.Id;
            studyGroup.CreateDate = DateTime.UtcNow;

            await _studyGroupRepository.Add(studyGroup);

            return RedirectToAction(nameof(Index));
        }
    }
}
