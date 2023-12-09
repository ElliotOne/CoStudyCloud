using Microsoft.AspNetCore.Mvc;

namespace CoStudyCloud.Controllers
{
    public class GroupsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
