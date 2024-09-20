using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ProductAPI.Core.DTOs;
using ProductAPI.Core.Models;

namespace ProductAPI.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<IdentityUser, UserDto>().ReverseMap();
            CreateMap<IdentityRole, RoleDto>().ReverseMap();

            // Additional mappings from DTO to Entity
            CreateMap<ProductDto, Product>();
            CreateMap<UserDto, IdentityUser>();
            CreateMap<RoleDto, IdentityRole>();
        }
    }
}
