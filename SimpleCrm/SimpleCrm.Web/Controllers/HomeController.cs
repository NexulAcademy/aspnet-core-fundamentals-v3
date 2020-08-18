using Microsoft.AspNetCore.Mvc;
using SimpleCrm.Web.Models.Home;

namespace SimpleCrm.Web.Controllers
{
    public class HomeController : Controller
    {
        private ICustomerData _customerData;
        private readonly IGreeter _greeter;

        public HomeController(ICustomerData customerData,
            IGreeter greeter)
        {
            _customerData = customerData;
            _greeter = greeter;
        }

        public IActionResult Index()
        {
            var model = new HomePageViewModel();
            model.Customers = _customerData.GetAll();
            model.CurrentMessage = _greeter.GetGreeting();
            return View(model);
        }
        public IActionResult Details(int id)
        {
            var customer = _customerData.Get(id);
            if (customer == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }
        public IActionResult Create()
        {
            return View();
        }
    }
}
