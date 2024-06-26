﻿using NemEcommerce.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NemEcommerce.Admin.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class NemEcommerceController : AbpControllerBase
{
    protected NemEcommerceController()
    {
        LocalizationResource = typeof(NemEcommerceResource);
    }
}
