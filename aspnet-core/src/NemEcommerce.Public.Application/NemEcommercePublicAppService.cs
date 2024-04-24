using System;
using System.Collections.Generic;
using System.Text;
using NemEcommerce.Localization;
using Volo.Abp.Application.Services;

namespace NemEcommerce.Public;

/* Inherit your application services from this class.
 */
public abstract class NemEcommercePublicAppService : ApplicationService
{
    protected NemEcommercePublicAppService()
    {
        LocalizationResource = typeof(NemEcommerceResource);
    }
}
