namespace ApiFaceUnah.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool Active { get; set; }
    }
}
