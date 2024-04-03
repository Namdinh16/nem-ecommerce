using System;
using System.Collections.Generic;
using System.Text;
using NemEcommerce.Localization;
using Volo.Abp.Application.Services;

namespace NemEcommerce;

/* Inherit your application services from this class.
 */
public abstract class NemEcommerceAppService : ApplicationService
{
    protected NemEcommerceAppService()
    {
        LocalizationResource = typeof(NemEcommerceResource);
    }
}
