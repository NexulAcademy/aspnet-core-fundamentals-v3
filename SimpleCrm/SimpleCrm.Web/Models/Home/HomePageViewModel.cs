using System.Collections.Generic;

namespace SimpleCrm.Web.Models.Home
{
    public class HomePageViewModel
    {
        public IEnumerable<Customer> Customers { get; set; }
    }
}
