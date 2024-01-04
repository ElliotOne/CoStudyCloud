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

        public IActionResult Index()
        {
            return View();
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
