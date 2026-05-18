using AutoMapper;
using ECommmerce.Api.Data;
using ECommmerce.Api.DTOs;
using ECommmerce.Api.Model;
using ECommmerce.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ECommmerce.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ProductService(AppDbContext context, IMapper mapper) { 
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _context.Products.ToListAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) {
                return null;
            }

            return _mapper.Map<ProductDTO?>(product);
        }

        public async Task<ProductDTO> CreateProductAsync(CreateProductDTO dto)
        {
            var product = _mapper.Map<Product>(dto);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO?> UpdateProductAsync(int id, CreateProductDTO dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) {
                return null;
            }

            _mapper.Map(dto, product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductDTO?>(product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
