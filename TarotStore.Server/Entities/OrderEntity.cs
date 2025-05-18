namespace TarotStore.Server.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        public int ProductId { get; set; }
        public ProductEntity Product { get; set; }
        public int Amount { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    }
}
