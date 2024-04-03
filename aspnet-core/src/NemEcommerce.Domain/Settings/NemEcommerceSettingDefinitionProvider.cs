using Volo.Abp.Settings;

namespace NemEcommerce.Settings;

public class NemEcommerceSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(NemEcommerceSettings.MySetting1));
    }
}
