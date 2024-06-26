﻿using AutoMapper;
using NemEcommerce.Admin.Catalog.Manufacturers;
using NemEcommerce.Admin.Catalog.ProductAttributes;
using NemEcommerce.Admin.Catalog.ProductCategories;
using NemEcommerce.Admin.Catalog.Products;
using NemEcommerce.Admin.System.Roles;
using NemEcommerce.Admin.System.Users;
using NemEcommerce.Manufactures;
using NemEcommerce.Orders;
using NemEcommerce.ProductAttributes;
using NemEcommerce.ProductCategories;
using NemEcommerce.Products;
using NemEcommerce.Roles;
using Volo.Abp.Identity;

namespace NemEcommerce.Admin;

public class NemEcommerceAdminApplicationAutoMapperProfile : Profile
{
    public NemEcommerceAdminApplicationAutoMapperProfile()
    {
        //Product Category
        CreateMap<ProductCategory, ProductCategoryDto>();
        CreateMap<ProductCategory, ProductCategoryInListDto>();
        CreateMap<CreateUpdateProductCategoryDto, ProductCategory>();


        //Product
        CreateMap<Product, ProductDto>();
        CreateMap<Product, ProductInListDto>();
        CreateMap<CreateUpdateProductDto, Product>();


        //Manufacturer
        CreateMap<Manufacturer, ManufacturerDto>();
        CreateMap<Manufacturer, ManufacturerInListDto>();
        CreateMap<CreateUpdateManufacturerDto, Manufacturer>();

        //Product Attributes
        CreateMap<ProductAttribute, ProductAttributeDto>();
        CreateMap<ProductAttribute, ProductAttributeInListDto>();
        CreateMap<CreateUpdateProductAttributeDto, ProductAttribute>();

        //Order
        CreateMap<Order, OrderDto>();
        CreateMap<Order, OrderInListDto>();
        CreateMap<CreateOrderDto, Order>();

        //Roles
        CreateMap<IdentityRole, RoleDto>().ForMember(x => x.Description,
            map => map.MapFrom(x => x.ExtraProperties.ContainsKey(RoleConsts.DescriptionFieldName)
            ?
            x.ExtraProperties[RoleConsts.DescriptionFieldName]
            :
            null));
        CreateMap<IdentityRole, RoleInListDto>().ForMember(x => x.Description,
            map => map.MapFrom(x => x.ExtraProperties.ContainsKey(RoleConsts.DescriptionFieldName)
            ?
            x.ExtraProperties[RoleConsts.DescriptionFieldName]
            :
            null)); 
        CreateMap<CreateUpdateRoleDto, IdentityRole>();


        //Users
        CreateMap<IdentityUser, UserDto>();
        CreateMap<IdentityUser, UserInListDto>();
    }
}
