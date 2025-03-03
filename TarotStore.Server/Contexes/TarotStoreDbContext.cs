using Microsoft.EntityFrameworkCore;
using TarotStore.Server.Entities;

namespace TarotStore.Server.Contexes
{
    public class TarotStoreDbContext : DbContext
    {
        public TarotStoreDbContext(DbContextOptions<TarotStoreDbContext> options) : base(options) { }

        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<OrderEntity> Order { get; set; }
        public DbSet<ProductTypeEntity> ProductType { get; set; }
        public DbSet<RoleEntity> Role { get; set; }
        public DbSet<UserByRoleEntity> UserByRole { get; set; }
        public DbSet<UserDetailsEntity> UserDetails { get; set; }
        public DbSet<UserEntity> User { get; set; }
    }
}
