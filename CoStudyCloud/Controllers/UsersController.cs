using Microsoft.AspNetCore.Mvc;

namespace CoStudyCloud.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
