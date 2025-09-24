using System.Reflection;
using ApplicationTask.Application.Common.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestMillion.Application.Common.Behaviours;
using TestMillion.Infrastructure;

namespace TestMillion.Application;

public static class DependencyInjectionApplication
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services,
    IConfiguration configuration
    )
  {
    
    services.AddInfrastructure(configuration);
    
    services.AddExceptionHandler<ValidationExceptionHandler>();
    services.AddExceptionHandler<BadRequestExceptionHandler>();
    services.AddExceptionHandler<NotFoundExceptionHandler>();
    services.AddExceptionHandler<InternalServerExceptionHandler>();
    services.AddExceptionHandler<GlobalExceptionHandler>();
    services.AddProblemDetails();
    
    services.AddAutoMapper(Assembly.GetExecutingAssembly());

    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), ServiceLifetime.Transient);

    services.AddMediatR(cfg =>
    {
      cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
      cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
    });

    return services;
  }
}