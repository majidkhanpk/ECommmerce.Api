using AutoMapper;
using ECommmerce.Api.DTOs;
using ECommmerce.Api.Model;

namespace ECommmerce.Api.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile() { 
            CreateMap<Product, ProductDTO>();
            CreateMap<CreateProductDTO, Product>();
        }
    }
}
