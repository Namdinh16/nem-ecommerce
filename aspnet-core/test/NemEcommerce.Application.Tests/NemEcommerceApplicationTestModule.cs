using Volo.Abp.Modularity;

namespace NemEcommerce;

[DependsOn(
    typeof(NemEcommerceApplicationModule),
    typeof(NemEcommerceDomainTestModule)
)]
public class NemEcommerceApplicationTestModule : AbpModule
{

}
