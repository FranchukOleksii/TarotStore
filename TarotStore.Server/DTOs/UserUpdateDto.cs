namespace TarotStore.Server.DTOs
{
    public class UserUpdateDto
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDay { get; set; }
        public decimal? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}
