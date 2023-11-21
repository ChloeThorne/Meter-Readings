namespace Meter_Readings_API.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Account() { }
        public Account(int id, string firstName, string lastName)
        {
            AccountId = id;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
