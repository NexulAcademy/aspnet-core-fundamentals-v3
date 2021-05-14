using System.Collections.Generic;

namespace SimpleCrm
{
    public interface ICustomerData
    {
        Customer Get(int id);
        /// <summary>
        /// Gets paged and sorted records for a given CRM account and status.
        /// </summary>
        /// <returns></returns>
        List<Customer> GetAll(CustomerListParameters listParameters);
        void Add(Customer customer);
        void Update(Customer customer);
        /// <summary>
        /// Marks an item as deleted, to be saved on next commit.
        /// </summary>
        /// <param name="item"></param>
        void Delete(Customer item);
        /// <summary>
        /// Saves changes to new or modified customers.
        /// </summary>
        void Commit();
    }
}
