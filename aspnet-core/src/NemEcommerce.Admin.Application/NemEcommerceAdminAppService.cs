using System;
using System.Collections.Generic;
using System.Text;
using NemEcommerce.Localization;
using Volo.Abp.Application.Services;

namespace NemEcommerce.Admin;

/* Inherit your application services from this class.
 */
public abstract class NemEcommerceAdminAppService : ApplicationService
{
    protected NemEcommerceAdminAppService()
    {
        LocalizationResource = typeof(NemEcommerceResource);
    }
}
