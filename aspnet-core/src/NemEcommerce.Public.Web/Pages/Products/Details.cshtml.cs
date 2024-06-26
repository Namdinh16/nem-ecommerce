using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NemEcommerce.Public.Catalog.ProductCategories;
using NemEcommerce.Public.Catalog.Products;
using System.Threading.Tasks;

namespace NemEcommerce.Public.Web.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly IProductsAppService _productsAppService;
        private readonly IProductCategoriesAppService _productCategoriesAppService;
        public DetailsModel(IProductsAppService productsAppService,
            IProductCategoriesAppService productCategoriesAppService)
        {
            _productsAppService = productsAppService;
            _productCategoriesAppService = productCategoriesAppService;
        }
        public ProductCategoryDto Category { get; set; }
        public ProductDto Product { get; set; }
        public async Task OnGetAsync(string categorySlug, string slug)
        {
            Category = await _productCategoriesAppService.GetBySlugAsync(categorySlug);
            Product = await _productsAppService.GetBySlugAsync(slug);
        }
    }
}
