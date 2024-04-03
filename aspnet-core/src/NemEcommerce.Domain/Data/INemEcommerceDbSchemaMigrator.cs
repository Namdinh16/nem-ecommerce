using System.Threading.Tasks;

namespace NemEcommerce.Data;

public interface INemEcommerceDbSchemaMigrator
{
    Task MigrateAsync();
}
