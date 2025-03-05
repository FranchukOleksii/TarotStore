using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TarotStore.Server.Contexes;
using TarotStore.Server.Entities;
using TarotStore.Server.Models.Enums;

namespace TarotStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly TarotStoreDbContext _context; 
        private readonly IConfiguration _configuration;

        public UserController(TarotStoreDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateUser(UserEntity user)
        {
            if (user == null) return BadRequest();

            var passwordHasher = new PasswordHasher<UserEntity>();
            user.PasswordHash = passwordHasher.HashPassword(user, user.PasswordHash);

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

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<UserEntity>>> GetUsers()
        {
            return await _context.User.ToListAsync();
        }

        [HttpGet("Id")]
        public async Task<ActionResult<UserEntity>> GetUser(int? Id)
        {
            if (Id == null) return BadRequest();
            var user = await _context.User.FindAsync(Id);
            if (user == null) return BadRequest();
            return user;
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateUser(UserEntity user)
        {
            if (user == null) return BadRequest();
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("Id")]
        public async Task<IActionResult> DeleteUser(int? Id)
        {
            if (Id == null) return BadRequest();
            var user = await _context.User.FindAsync(Id);
            if (user == null) return BadRequest();
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(string email, string password)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return Unauthorized("Invalid credentials");

            var passwordHasher = new PasswordHasher<UserEntity>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid credentials");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, "User") // Додай роль, якщо потрібно
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Термін дії токена
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });

            return Ok("Login successful");
        }
    }
}