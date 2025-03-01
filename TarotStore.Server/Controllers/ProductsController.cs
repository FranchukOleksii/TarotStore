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

        [HttpPost()]
        public async Task<IActionResult> CreateProduct(ProductEntity product) {
            if (product != null) {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<ProductEntity>>> GetProducts() {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("Id")]
        public async Task<ActionResult<ProductEntity>> GetProduct(int Id) {
            if (Id == null) return NotFound(); 
            var product = await _context.Products.FindAsync(Id);
            if (product == null) return NotFound();
            return product;
        }

        [HttpPut("Id")]
        public async Task<ActionResult<ProductEntity>> UpdateProduct(ProductEntity product) {
            if (product == null) return BadRequest();
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("Id")]
        public async Task<IActionResult> DeleteProduct(int Id) {
            if (Id == null) return NotFound();
            var product = await _context.Products.FindAsync(Id);
            if (product == null) return NotFound();
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}