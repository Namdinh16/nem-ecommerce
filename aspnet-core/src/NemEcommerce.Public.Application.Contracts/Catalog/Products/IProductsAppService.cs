using NemEcommerce.Public.Catalog.Products.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NemEcommerce.Public.Catalog.Products
{
    public interface IProductsAppService : IReadOnlyAppService
        <ProductDto,
        Guid,
        PagedResultRequestDto>
    {
        Task<PagedResult<ProductInListDto>> GetListFilterAsync(ProductListFilterDto input);
        Task<List<ProductInListDto>> GetListAllAsync();
        Task<string> GetThumbnailImageAsync(string fileName);
        Task<List<ProductAttributeValueDto>> GetListProductAttributeAllAsync(Guid productId);
        Task<PagedResult<ProductAttributeValueDto>> GetListProductAttributeAsync(ProductAttributeListFilterDto input);
        Task<List<ProductInListDto>> GetListTopSellerProductAsync(int numberOfRecords);
        Task<ProductDto> GetBySlugAsync(string slug);
    }
}
