using Microsoft.EntityFrameworkCore;
using TarotStore.Server.Entities;

namespace TarotStore.Server.Contexes
{
    public class TarotStoreDbContext : DbContext
    {
        public TarotStoreDbContext(DbContextOptions<TarotStoreDbContext> options) : base(options) { }

        public DbSet<ProductEntity> Products { get; set; }
    }
}
