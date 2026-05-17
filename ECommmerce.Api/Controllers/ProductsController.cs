using ECommmerce.Api.Data;
using ECommmerce.Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommmerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        private readonly List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Description = "A high-performance laptop", Price = 1200.00m, Stock = 10 },
            new Product { Id = 2, Name = "Keyboard", Description = "Mechanical keyboard with RGB lighting", Price = 150.00m, Stock = 50 },
            new Product { Id = 3, Name = "Headphone", Description = "Noise-cancelling over-ear headphones", Price = 180.00m, Stock = 30 },
            new Product { Id = 4, Name = "Mouse", Description = "Wireless ergonomic mouse", Price = 20.00m, Stock = 100 }
        };

        [HttpGet]
        public async Task<IActionResult> GetProducts() {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody]Product product) {
            if (product == null) { 
                return BadRequest();
            }
        
            product.Id = products.Max(p => p.Id) + 1;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product updateProduct) {
            if (updateProduct == null)
            {
                return BadRequest();
            }

            var existingProduct = await _context.Products.FindAsync(id);
            if ( existingProduct == null )
            {
                return NotFound();
            }

            existingProduct.Name = updateProduct.Name;
            existingProduct.Description = updateProduct.Description;
            existingProduct.Price = updateProduct.Price;
            existingProduct.Stock = updateProduct.Stock;

            await _context.SaveChangesAsync();
            return Ok(existingProduct);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id) { 
        
            var product = products.FirstOrDefault(x => x.Id == id);
            if ( product == null )
            {
                return NotFound();
            }

            products.Remove(product);

            return NoContent();
        }
    }

}
