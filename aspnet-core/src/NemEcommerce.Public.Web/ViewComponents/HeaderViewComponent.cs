using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using NemEcommerce.Public.Catalog.ProductCategories;
using NemEcommerce.Public.Web.Models;
using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;

namespace NemEcommerce.Public.Web.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        public readonly IProductCategoriesAppService _productCategoriesAppService;
        public readonly IDistributedCache<HeaderCacheItem> _distributedCache;
        public HeaderViewComponent(IProductCategoriesAppService productCategoriesAppService, IDistributedCache<HeaderCacheItem> distributedCache)
        {
            _productCategoriesAppService = productCategoriesAppService;
            _distributedCache = distributedCache;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cacheItem = await _distributedCache.GetOrAddAsync( NemEcommercePublicConsts.CacheKeys.HeaderData , async () =>
            {
                var model = await _productCategoriesAppService.GetListAllAsync();
                return new HeaderCacheItem()
                {
                    Categories = model
                };
            },
            () => new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddHours(12)
            });
            return View(cacheItem.Categories);
        }
    }
}
