using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TarotStore.Server.Contexes;
using TarotStore.Server.Entities;

namespace TarotStore.Server.Controllers
{
    //[Authorize] 
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly TarotStoreDbContext _context;

        public OrderController(TarotStoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderEntity>>> GetOrders()
        {
            return await _context.Order.ToListAsync();
        }

        [Authorize]
        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var orders = await _context.Order
                .Include(o => o.Product)
                .Where(o => o.UserId == userId)
                .Select(o => new
                {
                    o.Id,
                    o.Product.Name,
                    o.Product.Price,
                    o.Amount,
                    o.PriceAtPurchase
                })
                .ToListAsync();

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderEntity>> GetOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null) return NotFound();
            return order;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderEntity>> CreateOrder(OrderEntity order)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            order.UserId = userId;
            _context.Order.Add(order);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderEntity order)
        {
            if (id != order.Id) return BadRequest();
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //[Authorize(Roles = "Admin")] 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null) return NotFound();
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
