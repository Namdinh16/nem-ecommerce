using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace NemEcommerce.Data;

/* This is used if database provider does't define
 * INemEcommerceDbSchemaMigrator implementation.
 */
public class NullNemEcommerceDbSchemaMigrator : INemEcommerceDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
