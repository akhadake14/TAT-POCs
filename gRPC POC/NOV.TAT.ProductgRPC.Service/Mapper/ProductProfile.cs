using AutoMapper;
using NOV.TAT.ProductgRPC.Data.Models;
using ProductGRPCService;

namespace NOV.TAT.ProductgRPC.Service.Mapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductModel>();
            CreateMap<ProductModel, Product>();
        }
    }
}
