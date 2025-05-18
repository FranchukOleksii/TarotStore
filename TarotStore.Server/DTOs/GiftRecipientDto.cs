namespace TarotStore.Server.DTOs
{
    public class GiftRecipientDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }
}
