using AutoMapper;
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
        private readonly IMapper _mapper;

        public ProductsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts() {
            var products = await _context.Products.ToListAsync();
            var result = _mapper.Map<IEnumerable<ProductDTO>>(products);
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

            var result = _mapper.Map<ProductDTO>(product);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody]CreateProductDTO dto) {
            if (dto == null) { 
                return BadRequest();
            }

            var product = _mapper.Map<Product>(dto);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<ProductDTO>(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] CreateProductDTO dto) {
            if (dto == null)
            {
                return BadRequest();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null )
            {
                return NotFound();
            }
            _mapper.Map(dto, product);

            await _context.SaveChangesAsync();

            var result = _mapper.Map<ProductDTO>(product);

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
