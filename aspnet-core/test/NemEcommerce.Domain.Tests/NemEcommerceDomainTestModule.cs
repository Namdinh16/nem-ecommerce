using Volo.Abp.Modularity;

namespace NemEcommerce;

[DependsOn(
    typeof(NemEcommerceDomainModule),
    typeof(NemEcommerceTestBaseModule)
)]
public class NemEcommerceDomainTestModule : AbpModule
{

}
