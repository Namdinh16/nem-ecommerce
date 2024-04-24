using Volo.Abp.Modularity;

namespace NemEcommerce.Public;

[DependsOn(
    typeof(NemEcommercePublicApplicationModule),
    typeof(NemEcommerceDomainTestModule)
)]
public class NemEcommercePublicApplicationTestModule : AbpModule
{

}
