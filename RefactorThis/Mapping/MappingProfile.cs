using AutoMapper;
using RefactorThis.Models;
using RefactorThis.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductEntity, Product>();
            CreateMap<Product, ProductEntity>();
            CreateMap<CreateUpdateProductRequest, Product>();

            CreateMap<ProductOptionEntity, ProductOption>();
            CreateMap<ProductOption, ProductOptionEntity>();
            CreateMap<CreateUpdateOptionRequest, ProductOption>();
        }
    }
}
