using System;
using System.Globalization;

namespace SimpleCrm.WebApi.Models
{
    public class CustomerDisplayViewModel
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public InteractionMethod PreferredContactMethod { get; set; }
        public CustomerStatus Status { get; set; }
        public DateTime LastContactDate { get; set; }

        public CustomerDisplayViewModel() { }
        public CustomerDisplayViewModel(Customer source)
        {
            if (source == null)
                return;
            CustomerId = source.Id;
            FirstName = source.FirstName;
            LastName = source.LastName;
            EmailAddress = source.EmailAddress;
            PhoneNumber = source.PhoneNumber;
            Status = source.Status;
            PreferredContactMethod = source.PreferredContactMethod;
            LastContactDate = source.LastContactDate;
        }
    }
}
