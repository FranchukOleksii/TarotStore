using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TarotStore.Server.Contexes;
using TarotStore.Server.DTOs;
using TarotStore.Server.Entities;
using TarotStore.Server.Models.Enums;
using TarotStore.Server.Services;

namespace TarotStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly TarotStoreDbContext _context; 
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        public UserController(TarotStoreDbContext context, IConfiguration configuration, EmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateUser(UserRegisterDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Invalid data");

            var existing = await _context.User.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (existing != null)
                return BadRequest("Email already registered");

            var user = new UserEntity
            {
                Email = dto.Email
            };

            var passwordHasher = new PasswordHasher<UserEntity>();
            user.PasswordHash = passwordHasher.HashPassword(user, dto.Password);

            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();

            var isFirstUser = await _context.User.CountAsync() == 1;
            var roleToAssign = isFirstUser
                ? (int)RolesEnum.Admin
                : (int)RolesEnum.AuthenticatedUser;

            await _context.UserDetails.AddAsync(new UserDetailsEntity
            {
                UserId = user.Id,
                Name = dto.Name,
                Surname = dto.Surname,
                LastName = dto.LastName,
                BirthDay = dto.BirthDay,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address
            });

            await _context.UserByRole.AddAsync(new UserByRoleEntity
            {
                UserId = user.Id,
                RoleId = roleToAssign
            });

            await _context.SaveChangesAsync();

            var confirmToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Email));
            var confirmUrl = $"https://localhost:7056/api/user/confirm-email?token={confirmToken}";

            await _emailService.SendEmailAsync(user.Email, "Confirm your email",
                $"<p>Please confirm your email by clicking the link: <a href='{confirmUrl}'>Confirm Email</a></p>");

            return NoContent();
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            var email = Encoding.UTF8.GetString(Convert.FromBase64String(token));

            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return NotFound("User not found");

            user.IsEmailConfirmed = true;
            await _context.SaveChangesAsync();

            return Ok("✅ Email confirmed successfully.");
        }

        [Authorize]
        [HttpPost("resend-confirmation")]
        public async Task<IActionResult> ResendEmailConfirmation()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound("User not found");

            if (user.IsEmailConfirmed)
                return BadRequest("Email already confirmed");

            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Email));
            var confirmLink = $"https://localhost:7056/api/user/confirm-email?token={token}";
            var body = $"<h2>Confirm your email</h2><p>Click the link below:</p><a href='{confirmLink}'>Confirm Email</a>";

            await _emailService.SendEmailAsync(user.Email, "🔐 Confirm your email", body);

            return Ok("✅ Confirmation email resent");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.User
                .Select(user => new
                {
                    user.Id,
                    user.Email,
                    Role = _context.UserByRole
                        .Where(ur => ur.UserId == user.Id)
                        .Select(ur => ur.RoleId)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("Id")]
        public async Task<ActionResult<UserEntity>> GetUser(int? Id)
        {
            if (Id == null) return BadRequest();
            var user = await _context.User.FindAsync(Id);
            if (user == null) return BadRequest();
            return user;
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto updated)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var user = await _context.User
                .Include(u => u.UserDetails)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return NotFound();

            if (user.UserDetails == null)
            {
                user.UserDetails = new UserDetailsEntity
                {
                    UserId = userId
                };
                _context.UserDetails.Add(user.UserDetails);
            }

            user.UserDetails.Name = updated.Name;
            user.UserDetails.Surname = updated.Surname;
            user.UserDetails.LastName = updated.LastName;
            user.UserDetails.BirthDay = updated.BirthDay;
            user.UserDetails.PhoneNumber = updated.PhoneNumber;
            user.UserDetails.Address = updated.Address;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("changerole/{userId}")]
        public async Task<IActionResult> ChangeUserRole(int userId, int newRoleId)
        {
            var userRole = await _context.UserByRole.FirstOrDefaultAsync(ur => ur.UserId == userId);
            if (userRole == null) return NotFound("User role not found");

            userRole.RoleId = newRoleId;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null) return NotFound();

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [HttpDelete("deleteuserself")]
        public async Task<IActionResult> DeleteUserSelf()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var user = await _context.User.FindAsync(userId);
            if (user == null) return NotFound();

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

            var roleId = await _context.UserByRole
                .Where(ur => ur.UserId == user.Id)
                .Select(ur => ur.RoleId)
                .FirstOrDefaultAsync();

            var roleName = Enum.GetName(typeof(RolesEnum), roleId) ?? "AuthenticatedUser";

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, roleName)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var user = await _context.User
                .Include(u => u.UserDetails)    
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return NotFound();

            return Ok(new
            {
                user.Id,
                user.Email,
                user.IsEmailConfirmed,
                user.UserDetails?.Name,
                user.UserDetails?.Surname,
                user.UserDetails?.LastName,
                BirthDay = user.UserDetails?.BirthDay?.ToString("yyyy-MM-dd"),
                user.UserDetails?.PhoneNumber,
                user.UserDetails?.Address
            });
        }

        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _context.User.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound();

            var hasher = new PasswordHasher<UserEntity>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, dto.CurrentPassword);
            if (result == PasswordVerificationResult.Failed)
                return BadRequest("❌ Current password is incorrect.");

            user.PasswordHash = hasher.HashPassword(user, dto.NewPassword);
            await _context.SaveChangesAsync();

            return Ok("✅ Password changed successfully.");
        }

    }
}