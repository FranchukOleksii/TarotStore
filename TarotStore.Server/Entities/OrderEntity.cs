namespace TarotStore.Server.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
    }
}
