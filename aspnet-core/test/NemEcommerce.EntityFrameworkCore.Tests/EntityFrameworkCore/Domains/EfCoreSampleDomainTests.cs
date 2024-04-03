using NemEcommerce.Samples;
using Xunit;

namespace NemEcommerce.EntityFrameworkCore.Domains;

[Collection(NemEcommerceTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<NemEcommerceEntityFrameworkCoreTestModule>
{

}
