using Volo.Abp.Modularity;

namespace NemEcommerce;

public abstract class NemEcommerceApplicationTestBase<TStartupModule> : NemEcommerceTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
