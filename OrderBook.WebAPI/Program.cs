using OrderBook.Infrastructure.Hubs;
using OrderBook.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.ConfigureOptions();
builder.ConfigureApi();
builder.ConfigureInAppServices();

var app = builder.Build();

await app.RunMigrations();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(options => options
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<TradeHub>("/tradehub");

app.Run();
