using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using TarotStore.Server.Entities;
using TarotStore.Server.Models.Enums;

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

        public DbSet<GiftRecipientEntity> GiftRecipients { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleEntity>().HasData(
                Enum.GetValues(typeof(RolesEnum))
                .Cast<RolesEnum>()
                .Select(r => new RoleEntity { Id = (int)r, RoleName = r.ToString() })
                .ToList()
            );

            modelBuilder.Entity<UserByRoleEntity>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserByRoles)
                .HasForeignKey(ur => ur.UserId)       
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserDetailsEntity>()
                .HasOne(d => d.User)          
                .WithOne(u => u.UserDetails)
                .HasForeignKey<UserDetailsEntity>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderEntity>()
                .HasOne(o => o.Product)
                .WithMany()
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GiftRecipientEntity>()
                .HasOne(g => g.User)
                .WithMany()
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
