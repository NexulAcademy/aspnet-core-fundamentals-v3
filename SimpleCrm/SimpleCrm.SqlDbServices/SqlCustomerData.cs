using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

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

        public List<Customer> GetByStatus(CustomerStatus status, 
            int pageIndex, int take, string orderBy)
        {
            var sortableFields = new string[] { "FIRSTNAME", "LASTNAME", "EMAILADDRESS", "PHONENUMBER", "STATUS", "LASTCONTACTDATE" };
            var fields = (orderBy ?? "").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var field in fields)
            {
                var x = field.Trim().ToUpper();
                var parts = x.Split(' ');
                if (parts.Length > 2)
                    throw new ArgumentException("Invalid sort option " + x);
                if (parts.Length > 1 && parts[1].ToUpper() != "DESC" && parts[1].ToUpper() != "ASC")
                    throw new ArgumentException("Invalid sort direction " + x);
                if (!sortableFields.Contains(x))
                    throw new ArgumentException("Invalid sort field " + x);
            } //all sort requested fields are valid.
            return _context.Customers
                .Where(x => x.Status == status)
                .OrderBy(orderBy) //validated above to nothing unexpected, this is OK now
                .Skip(pageIndex * take)
                .Take(take)
                .ToList();
        }

        public void Add(Customer customer)
        {
            _context.Customers.Add(customer);
        }
        public void Update(Customer customer)
        {
            //Update is not needed, since changes are tracked by EF
        }

        public void Delete(Customer item)
        {
            _context.Remove(item);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
