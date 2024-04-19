using AutoMapper;
using NemEcommerce.Admin.Manufacturers;
using NemEcommerce.Admin.ProductAttributes;
using NemEcommerce.Admin.ProductCategories;
using NemEcommerce.Admin.Products;
using NemEcommerce.Admin.Roles;
using NemEcommerce.Manufactures;
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
    }
}
