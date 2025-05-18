using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TarotStore.Server.Contexes;
using TarotStore.Server.DTOs;
using TarotStore.Server.Entities;

namespace TarotStore.Server.Controllers
{
    [ApiController]
    [Route("api/gift-recipients")]
    [Authorize]
    public class GiftRecipientController : ControllerBase
    {
        private readonly TarotStoreDbContext _context;

        public GiftRecipientController(TarotStoreDbContext context)
        {
            _context = context;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Create(GiftRecipientCreateDto dto)
        {
            var recipient = new GiftRecipientEntity
            {
                Name = dto.FullName,
                Address = dto.Address,
                PhoneNumber = dto.Phone,
                UserId = GetUserId()
            };

            _context.GiftRecipients.Add(recipient);
            await _context.SaveChangesAsync();

            return Ok(new GiftRecipientDto
            {
                Id = recipient.Id,
                FullName = recipient.Name,
                Address = recipient.Address,
                Phone = recipient.PhoneNumber
            });
        }

        [HttpGet]
        public async Task<ActionResult<List<GiftRecipientDto>>> GetAll()
        {
            var recipients = await _context.GiftRecipients
                .Where(r => r.UserId == GetUserId())
                .Select(r => new GiftRecipientDto
                {
                    Id = r.Id,
                    FullName = r.Name,
                    Address = r.Address,
                    Phone = r.PhoneNumber
                })
                .ToListAsync();

            return Ok(recipients);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = GetUserId();

            var recipient = await _context.GiftRecipients
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

            if (recipient == null)
                return NotFound();

            _context.GiftRecipients.Remove(recipient);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
