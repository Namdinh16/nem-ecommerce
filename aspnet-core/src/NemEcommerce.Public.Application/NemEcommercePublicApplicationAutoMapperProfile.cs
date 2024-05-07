using AutoMapper;
using NemEcommerce.Manufactures;
using NemEcommerce.Orders;
using NemEcommerce.ProductAttributes;
using NemEcommerce.ProductCategories;
using NemEcommerce.Products;
using NemEcommerce.Public.Catalog.Manufacturers;
using NemEcommerce.Public.Catalog.ProductAttributes;
using NemEcommerce.Public.Catalog.ProductCategories;
using NemEcommerce.Public.Catalog.Products;
using NemEcommerce.Public.Orders;

namespace NemEcommerce.Public;

public class NemEcommercePublicApplicationAutoMapperProfile : Profile
{
    public NemEcommercePublicApplicationAutoMapperProfile()
    {
        CreateMap<ProductCategory, ProductCategoryDto>();
        CreateMap<ProductCategory, ProductCategoryInListDto>();


        //Product
        CreateMap<Product, ProductDto>();
        CreateMap<Product, ProductInListDto>();


        //Manufacturer
        CreateMap<Manufacturer, ManufacturerDto>();
        CreateMap<Manufacturer, ManufacturerInListDto>();

        //Product Attributes
        CreateMap<ProductAttribute, ProductAttributeDto>();
        CreateMap<ProductAttribute, ProductAttributeInListDto>();

        CreateMap<Order, OrderDto>();
    }
}
