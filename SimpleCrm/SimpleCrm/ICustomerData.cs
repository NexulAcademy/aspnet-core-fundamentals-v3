using System.Collections.Generic;

namespace SimpleCrm
{
    public interface ICustomerData
    {
        IEnumerable<Customer> GetAll();
        Customer Get(int id);
    }
}
