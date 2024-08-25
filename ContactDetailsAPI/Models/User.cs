using System.ComponentModel.DataAnnotations;

namespace ContactDetailsAPI.Models
{
    public class User
    {
        public int? UserID { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string PostalCode { get; set; }
        public string? Password { get; set; }
        public string? UserStatus { get; set; }
    }

    public class RegisterFilter
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserStatusFilter
    {
        public int UserID { get; set; }
        public string? UserStatus { get; set; }
    }
}
