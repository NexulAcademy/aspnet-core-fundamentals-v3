using System.Collections.Generic;

namespace SimpleCrm
{
    public class InMemoryCustomerData : ICustomerData
    {
        IList<Customer> _customers; //not thread safe - only ok for development, single user

        public InMemoryCustomerData()
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

        public IEnumerable<Customer> GetAll()
        {
            return _customers;
        }
    }
}
