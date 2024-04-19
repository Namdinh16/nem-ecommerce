using NemEcommerce.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NemEcommerce.Admin.Permissions;

public class NemEcommercePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var systemGroup = context.AddGroup(NemEcommercePermissions.SystemGroupName);
        var rolePermission = systemGroup.AddPermission(NemEcommercePermissions.Role.Default, L("Permission:System.Role"));
        rolePermission.AddChild(NemEcommercePermissions.Role.Create, L("Permission: System.Role.Create"));
        rolePermission.AddChild(NemEcommercePermissions.Role.Update, L("Permission: System.Role.Update"));
        rolePermission.AddChild(NemEcommercePermissions.Role.Delete, L("Permission: System.Role.Delete"));

        var userPermission = systemGroup.AddPermission(NemEcommercePermissions.User.Default, L("Permission:System.User"));
        userPermission.AddChild(NemEcommercePermissions.User.Create, L("Permission: System.User.Create"));
        userPermission.AddChild(NemEcommercePermissions.User.Update, L("Permission: System.User.Update"));
        userPermission.AddChild(NemEcommercePermissions.User.Delete, L("Permission: System.User.Delete"));



        var catalogGroup = context.AddGroup(NemEcommercePermissions.CatalogGroupName);
        var productPermission = catalogGroup.AddPermission(NemEcommercePermissions.Product.Default, L("Permission:Catalog.Product"));
        productPermission.AddChild(NemEcommercePermissions.Product.Create, L("Permission: Catalog.Product.Create"));
        productPermission.AddChild(NemEcommercePermissions.Product.Update, L("Permission: Catalog.Product.Update"));
        productPermission.AddChild(NemEcommercePermissions.Product.Delete, L("Permission: Catalog.Product.Delete"));
        productPermission.AddChild(NemEcommercePermissions.Product.AttributeManage, L("Permission: Catalog.Product.AttributeManage"));


        var attributePermission = catalogGroup.AddPermission(NemEcommercePermissions.Attribute.Default, L("Permission:Catalog.Attribute"));
        attributePermission.AddChild(NemEcommercePermissions.Attribute.Create, L("Permission: Catalog.Attribute.Create"));
        attributePermission.AddChild(NemEcommercePermissions.Attribute.Update, L("Permission: Catalog.Attribute.Update"));
        attributePermission.AddChild(NemEcommercePermissions.Attribute.Delete, L("Permission: Catalog.Attribute.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<NemEcommerceResource>(name);
    }
}
