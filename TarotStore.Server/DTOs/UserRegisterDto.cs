namespace TarotStore.Server.DTOs
{
    public class UserRegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDay { get; set; }
        public decimal? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}
