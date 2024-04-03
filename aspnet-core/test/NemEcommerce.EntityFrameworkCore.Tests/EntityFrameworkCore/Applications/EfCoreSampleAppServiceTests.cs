using NemEcommerce.Samples;
using Xunit;

namespace NemEcommerce.EntityFrameworkCore.Applications;

[Collection(NemEcommerceTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<NemEcommerceEntityFrameworkCoreTestModule>
{

}
