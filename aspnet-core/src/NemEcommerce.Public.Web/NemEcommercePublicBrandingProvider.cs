using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace NemEcommerce.Public.Web;

[Dependency(ReplaceServices = true)]
public class NemEcommercePublicBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Public";
}
