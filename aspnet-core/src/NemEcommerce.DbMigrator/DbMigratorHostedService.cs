﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NemEcommerce.Data;
using NemEcommerce.Seeding;
using Serilog;
using Volo.Abp;
using Volo.Abp.Data;

namespace NemEcommerce.DbMigrator;

public class DbMigratorHostedService : IHostedService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IConfiguration _configuration;

    public DbMigratorHostedService(IHostApplicationLifetime hostApplicationLifetime, IConfiguration configuration)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _configuration = configuration;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var application = await AbpApplicationFactory.CreateAsync<NemEcommerceDbMigratorModule>(options =>
        {
           options.Services.ReplaceConfiguration(_configuration);
           options.UseAutofac();
           options.Services.AddLogging(c => c.AddSerilog());
           options.AddDataMigrationEnvironment();
        }))
        {
            await application.InitializeAsync();

            await application
                .ServiceProvider
                .GetRequiredService<NemEcommerceDbMigrationService>()
                .MigrateAsync();

            await application
                .ServiceProvider
                .GetRequiredService<IdentityDataSeeder>()
                .SeedAsync("admin@gmail.com", "Nem@123");

            await application.ShutdownAsync();

            _hostApplicationLifetime.StopApplication();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
