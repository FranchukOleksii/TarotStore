namespace TarotStore.Server.Entities
{
    public class UserDetailsEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDay { get; set; }
        public decimal? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}
