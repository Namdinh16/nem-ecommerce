using NemEcommerce.Public.Catalog.ProductCategories;
using NemEcommerce.Public.Catalog.Products;
using System.Collections.Generic;


namespace NemEcommerce.Public.Web.Models
{
    public class HomeCacheItem
    {
        public List<ProductCategoryInListDto> Categories { get; set; }
        public List<ProductInListDto> TopSellerProducts { get; set; }
    }
}
