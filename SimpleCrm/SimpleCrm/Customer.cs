﻿// /SimpleCrm/Customer.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleCrm
{
    public class Customer
    {
        public int Id { get; set; }
        [MaxLength(50)]
        [Required()]
        public string FirstName { get; set; }
        [MinLength(1), MaxLength(50)]
        [Required()]
        public string LastName { get; set; }
        [MinLength(7), MaxLength(12)]
        public string PhoneNumber { get; set; }
        [MaxLength(400)]
        public string EmailAddress { get; set; }
        public bool OptInNewsletter { get; set; }
        public CustomerType Type { get; set; }
        public InteractionMethod PreferredContactMethod { get; set; }
        public CustomerStatus Status { get; set; }
        /// <summary>
        /// The last date and time this contact was updated.
        /// TODO: rename to better align with purpose
        /// </summary>
        public DateTime LastContactDate { get; set; }
    }
}
