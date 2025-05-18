using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TarotStore.Server.Contexes;
using TarotStore.Server.Entities;

namespace TarotStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly TarotStoreDbContext _context;

        public ProductsController(TarotStoreDbContext context)
        {
            _context = context;
        }

        //[Authorize]
        [HttpPost()]
        public async Task<IActionResult> CreateProduct(ProductEntity product) {
            if (product == null) return BadRequest();
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //[Authorize]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<ProductEntity>>> GetProducts() {
            return await _context.Products.ToListAsync();
        }

        //[Authorize]
        [HttpGet("Id")]
        public async Task<IActionResult> GetProduct(int? Id) {
            if (Id == null) return NotFound(); 
            var product = await _context.Products.FindAsync(Id);
            if (product == null) return NotFound();
            return NoContent();
        }

        //[Authorize]
        [HttpPut()]
        public async Task<IActionResult> UpdateProduct(ProductEntity product) {
            if (product == null) return BadRequest();
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        //[Authorize]
        [HttpDelete("Id")]
        public async Task<IActionResult> DeleteProduct(int? Id) {
            if (Id == null) return NotFound();
            var product = await _context.Products.FindAsync(Id);
            if (product == null) return NotFound();
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}