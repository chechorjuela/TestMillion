using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TestMillion.Domain.Interfaces;
using TestMillion.Domain.Interfaces.Base;
using TestMillion.Infrastructure.Persistence.MongoDB;
using TestMillion.Infrastructure.Repositories;

namespace TestMillion.Infrastructure;

public static class DependencyInjectionInfrastructure
{
  public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
  {
    services.Configure<MongoDbSettings>(configuration.GetSection("MongoDb"));

    services.AddScoped(typeof(IBaseRepository<>), typeof(GenericRepository<>));
    services.AddScoped<IPropertyTraceRepository, PropertyTraceRepository>();
    services.AddScoped<IPropertyRepository, PropertyRepository>();
    services.AddScoped<IOwnerRepository, OwnerRepository>();
    services.AddScoped<IPropertyImageRepository, PropertyImageRepository>();

    services.AddHttpContextAccessor();

    services.AddSingleton(TimeProvider.System);
    
    return services;
  }
  /*
  private static void ConfigureSerilog()
  {
    Log.Logger = new LoggerConfiguration()
      .Enrich.With(new LoggingEnricher())
      .MinimumLevel.Debug()
      .WriteTo.Console(
        outputTemplate: "{Timestamp:HH:mm} [{Level}] ({ThreadId}) {Message}{NewLine}{Exception}"
      )
      .CreateLogger();
  }*/
}

