using Volo.Abp.Modularity;

namespace NemEcommerce.Admin;

[DependsOn(
    typeof(NemEcommerceAdminApplicationModule),
    typeof(NemEcommerceDomainTestModule)
)]
public class NemEcommerceApplicationTestModule : AbpModule
{

}
