using Volo.Abp.Modularity;

namespace NemEcommerce.Admin;

public abstract class NemEcommerceApplicationTestBase<TStartupModule> : NemEcommerceTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
