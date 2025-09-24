using TestMillion.Application;
using TestMillion.Shared.Core;

var builder = WebApplication.CreateBuilder(args);

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

