using Microsoft.AspNetCore.Mvc;
using SimpleCrm.Web.Models.Account;

namespace SimpleCrm.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Register(RegisterUserViewModel model)
        {
            return NoContent(); //TODO: validate model, register the user
        }
    }
}
