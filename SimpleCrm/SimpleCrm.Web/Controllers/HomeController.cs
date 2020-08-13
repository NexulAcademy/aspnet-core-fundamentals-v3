using Microsoft.AspNetCore.Mvc;

namespace SimpleCrm.Web.Controllers
{
    public class HomeController : Controller
    {
        private ICustomerData _customerData;

        public HomeController(ICustomerData customerData)
        {
            _customerData = customerData;
        }

        public IActionResult Index()
        {
            var model = _customerData.GetAll();
            return View(model);
        }
    }
}
