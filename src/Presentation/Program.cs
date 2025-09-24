using TestMillion.Application;
using TestMillion.Shared.Core;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/testmillion-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7)
    .Enrich.WithThreadId()
    .Enrich.WithEnvironmentName()
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

EngineContext.Create();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

EngineContext.Current.Configure(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

