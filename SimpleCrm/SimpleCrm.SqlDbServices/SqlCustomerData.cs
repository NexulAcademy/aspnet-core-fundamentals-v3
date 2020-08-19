using System.Collections.Generic;
using System.Linq;

namespace SimpleCrm.SqlDbServices
{
    public class SqlCustomerData : ICustomerData
    {
        private SimpleCrmDbContext _context;
        public SqlCustomerData(SimpleCrmDbContext context)
        {
            _context = context;
        }

        public Customer Get(int id)
        {
            return _context.Customers.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }

        public void Save(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }
    }
}
