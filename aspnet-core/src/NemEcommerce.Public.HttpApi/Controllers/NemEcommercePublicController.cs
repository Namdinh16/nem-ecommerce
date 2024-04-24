using NemEcommerce.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NemEcommerce.Public.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class NemEcommercePublicController : AbpControllerBase
{
    protected NemEcommercePublicController()
    {
        LocalizationResource = typeof(NemEcommerceResource);
    }
}
