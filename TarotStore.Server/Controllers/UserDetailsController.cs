using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TarotStore.Server.Contexes;
using TarotStore.Server.Entities;
using TarotStore.Server.Models.Enums;

namespace TarotStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : Controller
    {
        private readonly TarotStoreDbContext _context;
        public UserDetailsController(TarotStoreDbContext context)
        {
            _context = context;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateUserDetail(int id, UserEntity user)
        {
            if (id == null || user == null) return BadRequest();
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
            await _context.UserDetails.AddAsync(new UserDetailsEntity
            {
                UserId = user.Id,
                Name = null,
                Surname = null,
                LastName = null,
                BirthDay = DateTime.Today,
                PhoneNumber = 0,
                Address = null
            });
            await _context.UserByRole.AddAsync(new UserByRoleEntity
            {
                UserId = user.Id,
                RoleId = (int)RolesEnum.AuthenticatedUser
            });
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //[HttpGet()]
        //public async Task<ActionResult<IEnumerable<UserEntity>>> GetUserDetails()
        //{
        //    return await _context.User.ToListAsync();
        //}

        [HttpGet("Id")]
        public async Task<ActionResult<UserEntity>> GetUserDetail(int? Id)
        {
            if (Id == null) return BadRequest();
            var user = await _context.User.FindAsync(Id);
            if (user == null) return BadRequest();
            return user;
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateUserDetail(UserEntity user)
        {
            if (user == null) return BadRequest();
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("Id")]
        public async Task<IActionResult> DeleteUserDetail(int? Id)
        {
            if (Id == null) return BadRequest();
            var user = await _context.User.FindAsync(Id);
            if (user == null) return BadRequest();
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}