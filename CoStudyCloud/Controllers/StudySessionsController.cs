using Microsoft.AspNetCore.Mvc;

namespace CoStudyCloud.Controllers
{
    public class StudySessions : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
