using Microsoft.AspNetCore.Mvc;
using SimpleCrm.Web.Models;

namespace SimpleCrm.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = new CustomerModel
            {
                Id= 1,
                FirstName = "Michael",
                LastName = "Lang",
                PhoneNumber = "314-555-1234"
            };
            return View(model);
        }
    }
}
