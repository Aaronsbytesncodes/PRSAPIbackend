using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRSBackendAB.models
{
    [Table("User")]
    public class User
    {
        public int ID { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool? Reviewer { get; set; }
        public bool? Admin { get; set; }

      
        //public string PasswordHash { get; set; }
    }
}
