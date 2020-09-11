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

        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }
        public void Update(Customer customer)
        {
            //Update is not needed, since changes are tracked by EF
            _context.SaveChanges();
        }
    }
}
