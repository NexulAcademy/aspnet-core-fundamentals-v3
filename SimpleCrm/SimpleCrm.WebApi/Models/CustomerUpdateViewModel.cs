namespace SimpleCrm.WebApi.Models
{
    /// <summary>
    /// Update is currently the same as update model, but in the future they will likely diverge.
    /// </summary>
    public class CustomerUpdateViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public InteractionMethod PreferredContactMethod { get; set; }
    }
}
