using AutoMapper;
using ECommmerce.Api.Data;
using ECommmerce.Api.DTOs;
using ECommmerce.Api.Model;
using ECommmerce.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommmerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] ProductQueryParameters query) {
            var (data ,totalcount) = await _productService.GetAllProductsAsync(query);
            var response = new
            {
                data,
                totalcount,
                query.Page,
                query.PageSize
            };
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody]CreateProductDTO dto) {
            if (dto == null) { 
                return BadRequest();
            }

            var product = await _productService.CreateProductAsync(dto);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] CreateProductDTO dto) {
            if (dto == null)
            {
                return BadRequest();
            }

            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
                
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id) { 
        
            var product = await _productService.DeleteProductAsync(id);
            if ( !product)
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpGet("test-error")]
        public IActionResult TestError()
        {
            throw new Exception("This is a test exception from product controller.");
        }
    }

}
