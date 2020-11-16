using System.Collections.Generic;

namespace SimpleCrm
{
    public interface ICustomerData
    {
        IEnumerable<Customer> GetAll();
        Customer Get(int id);
        /// <summary>
        /// Gets paged and sorted records for a given CRM account and status.
        /// </summary>
        /// <param name="status">The desired status filter</param>
        /// <param name="pageIndex">The zero based page number</param>
        /// <param name="take">The max number of records to take (page size)</param>
        /// <param name="orderBy">The property name to order by and optional direction. (null for default order)</param>
        /// <returns></returns>
        List<Customer> GetByStatus(CustomerStatus status,
            int pageIndex, int take, string orderBy);
        void Add(Customer customer);
        void Update(Customer customer);
        /// <summary>
        /// Saves changes to new or modified customers.
        /// </summary>
        void Commit();
    }
}
