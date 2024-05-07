using NemEcommerce.Public.Catalog.ProductCategories;
using NemEcommerce.Public.Catalog.Products;
using System.Collections.Generic;


namespace NemEcommerce.Public.Web.Models
{
    public class HeaderCacheItem
    {
        public List<ProductCategoryInListDto> Categories { get; set; }
    }
}
