namespace ContactDetailsAPI.Models
{
    public class Login
    {
        public int UserId { get; set; } 
        public string Username { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
    }
}
