using ECommmerce.Api.Data;
using ECommmerce.Api.DTOs;
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

            var result = products.Select(p => new ProductDTO { 
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var result = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody]CreateProductDTO dto) {
            if (dto == null) { 
                return BadRequest();
            }

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock
            };

            //product.Id = products.Max(p => p.Id) + 1;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var result = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock
            };

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] CreateProductDTO dto) {
            if (dto == null)
            {
                return BadRequest();
            }

            var existingProduct = await _context.Products.FindAsync(id);
            if ( existingProduct == null )
            {
                return NotFound();
            }

            existingProduct.Name = dto.Name;
            existingProduct.Description = dto.Description;
            existingProduct.Price = dto.Price;
            existingProduct.Stock = dto.Stock;

            await _context.SaveChangesAsync();

            var result = new ProductDTO
            {
                Id = existingProduct.Id,
                Name = existingProduct.Name,
                Description = existingProduct.Description,
                Price = existingProduct.Price,
                Stock = existingProduct.Stock
            };

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id) { 
        
            var product = await _context.Products.FindAsync(id);
            if ( product == null )
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
