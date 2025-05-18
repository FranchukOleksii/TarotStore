namespace TarotStore.Server.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsEmailConfirmed { get; set; } = false;
        public virtual UserDetailsEntity UserDetails { get; set; }
        public virtual ICollection<UserByRoleEntity> UserByRoles { get; set; } = new List<UserByRoleEntity>();
    }
}
