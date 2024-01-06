using CoStudyCloud.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoStudyCloud.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("/Home/Error/{errorCode}")]
        public IActionResult Error(int errorCode = 404)
        {
            var errorIndexViewModel = new ErrorViewModel()
            {
                ErrorCode = errorCode
            };

            return View(errorIndexViewModel);
        }
    }
}
