using Microsoft.AspNetCore.Mvc;

namespace U3RazasPerros.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        [Route("admin")]
        [Route("admin/Home")]
        [Route("admin/Home/Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
