using AutoMapper;
using CoStudyCloud.Core.Models;
using CoStudyCloud.Core.Repositories;
using CoStudyCloud.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoStudyCloud.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetUsers();

            var userIndexViewModel = new UserIndexViewModel()
            {
                UserEntryViewModels = _mapper.Map<
                    IEnumerable<User>, IEnumerable<UserEntryViewModel>>(users)
            };

            return View(userIndexViewModel);
        }
    }
}
