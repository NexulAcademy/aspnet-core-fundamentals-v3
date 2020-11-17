using System.Collections.Generic;
using System.Linq;

namespace SimpleCrm
{
    public class InMemoryCustomerData : ICustomerData
    {
        static IList<Customer> _customers; //not thread safe - only ok for development, single user

        static InMemoryCustomerData()
        {
            _customers = new List<Customer>
                {
                    new Customer { Id =1, FirstName ="Bob", LastName = "Jones", PhoneNumber = "555-555-2345" },
                    new Customer { Id =2, FirstName ="Jane", LastName = "Smith", PhoneNumber = "555-555-5256" },
                    new Customer { Id =3, FirstName ="Mike", LastName = "Doe", PhoneNumber = "555-555-8547" },
                    new Customer { Id =4, FirstName ="Karen", LastName = "Jamieson", PhoneNumber = "555-555-9134" },
                    new Customer { Id =5, FirstName ="James", LastName = "Dean", PhoneNumber = "555-555-7245" },
                    new Customer { Id =6, FirstName ="Michelle", LastName = "Leary", PhoneNumber = "555-555-3457" }
                };
        }

        public Customer Get(int id)
        {
            return _customers.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _customers;
        }

        public List<Customer> GetAll(int pageIndex, int take, string orderBy)
        {
            return _customers
                .Skip(pageIndex * take)
                .Take(take)
                // no ordering yet
                .ToList();
        }
        public void Add(Customer customer)
        {
            customer.Id = _customers.Max(x => x.Id) + 1;
            _customers.Add(customer);
        }
        public void Update(Customer customer)
        {
            var old = _customers.FirstOrDefault(x => x.Id == customer.Id);
            _customers.Remove(old);
            _customers.Add(customer);
        }

        public void Delete(Customer item)
        {
            _customers = _customers.Where(x => x.Id != item.Id).ToList();
        }

        public void Commit()
        {
            //nothing to do here. no transaction support
        }
    }
}
