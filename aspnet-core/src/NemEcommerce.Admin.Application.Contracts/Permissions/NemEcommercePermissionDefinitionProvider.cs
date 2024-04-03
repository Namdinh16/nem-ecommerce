using NemEcommerce.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NemEcommerce.Admin.Permissions;

public class NemEcommercePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(NemEcommercePermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(NemEcommercePermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<NemEcommerceResource>(name);
    }
}
