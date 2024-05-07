using NemEcommerce.Public.Catalog.Products;

namespace NemEcommerce.Public.Web.Models
{
    public class CartItem
    {
        public ProductDto Product { get; set; }
        public int Quantity { get; set; }
    }
}
