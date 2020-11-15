using System.Collections.Generic;

namespace SimpleCrm
{
    public interface ICustomerData
    {
        IEnumerable<Customer> GetAll();
        Customer Get(int id);
        void Add(Customer customer);
        void Update(Customer customer);
        /// <summary>
        /// Saves changes to new or modified customers.
        /// </summary>
        void Commit();  // <- NEW!
    }
}
