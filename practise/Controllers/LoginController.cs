using Microsoft.AspNetCore.Mvc;

namespace practise.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Show(string account, string password)
        {
            ViewBag.Account = account;
            ViewBag.Password = password;
            return View();
        }
    }
}
