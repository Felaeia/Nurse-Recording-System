using Microsoft.AspNetCore.Mvc;

namespace NurseRecordingSystem.Controllers
{
    public class CreateUserFormController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
