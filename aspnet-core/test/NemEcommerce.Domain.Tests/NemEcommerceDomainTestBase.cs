using Volo.Abp.Modularity;

namespace NemEcommerce;

/* Inherit from this class for your domain layer tests. */
public abstract class NemEcommerceDomainTestBase<TStartupModule> : NemEcommerceTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
