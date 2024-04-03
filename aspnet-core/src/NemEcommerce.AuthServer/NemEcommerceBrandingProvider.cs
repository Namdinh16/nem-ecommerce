using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace NemEcommerce;

[Dependency(ReplaceServices = true)]
public class NemEcommerceBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "NemEcommerce";
}
