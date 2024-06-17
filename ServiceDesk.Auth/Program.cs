using ServiceDesk.Auth.Settings;
using ServiceDesk.Data.Context;
using ServiceDesk.Util.Service;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddOptions<JwtSettings>()
    .Configure<IConfiguration>((settings, config) =>
    {
        config.GetSection(JwtSettings.Section).Bind(settings);
    });

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Database") 
    ?? throw new InvalidOperationException("Database connection string is required");

builder.Services.AddPasswordHasher();
builder.Services.AddPersistenceContext(connectionString);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapControllers();

await app.RunAsync();
