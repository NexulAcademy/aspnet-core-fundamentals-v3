using System.Collections.Generic;

namespace SimpleCrm.Web.Models.Home
{
    public class HomePageViewModel
    {
        public string CurrentMessage { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
    }
}
