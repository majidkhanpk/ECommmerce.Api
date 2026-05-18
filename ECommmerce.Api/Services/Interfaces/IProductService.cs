using ECommmerce.Api.DTOs;

namespace ECommmerce.Api.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();

        Task<ProductDTO?> GetProductByIdAsync(int id);

        Task<ProductDTO> CreateProductAsync(CreateProductDTO dto);

        Task<ProductDTO?> UpdateProductAsync(int id, CreateProductDTO dto);

        Task<bool> DeleteProductAsync(int id);
    }
}
