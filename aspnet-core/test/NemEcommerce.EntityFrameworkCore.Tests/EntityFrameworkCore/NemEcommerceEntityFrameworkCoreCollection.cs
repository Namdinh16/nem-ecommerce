using Xunit;

namespace NemEcommerce.EntityFrameworkCore;

[CollectionDefinition(NemEcommerceTestConsts.CollectionDefinitionName)]
public class NemEcommerceEntityFrameworkCoreCollection : ICollectionFixture<NemEcommerceEntityFrameworkCoreFixture>
{

}
