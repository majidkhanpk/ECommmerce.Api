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

        public async Task<(IEnumerable<ProductDTO> Data, int TotalCount)> GetAllProductsAsync(ProductQueryParameters query)
        {
            var productQuery = _context.Products.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.Search)) { 
                productQuery = productQuery.Where(p => p.Name.Contains(query.Search));
            }

            if (query.MinPrice.HasValue) {
                productQuery = productQuery.Where(p => p.Price >= query.MinPrice.Value);
            }

            if (query.MaxPrice.HasValue)
            {
                productQuery = productQuery.Where(p => p.Price <= query.MaxPrice.Value);
            }

            var totalCount = await productQuery.CountAsync();

            productQuery = query.SortBy?.ToLower() switch
            {
                "name" => query.descending == true ? productQuery.OrderByDescending(p => p.Name) : productQuery.OrderBy(p => p.Name),
                "price" => query.descending == true ? productQuery.OrderByDescending(p => p.Price) : productQuery.OrderBy(p => p.Price),
                _ => productQuery.OrderBy(p => p.Id)
            };

            var products = await productQuery
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

           var data = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return (data, totalCount);
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
