using Microsoft.IdentityModel.JsonWebTokens;
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

JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("Database") 
    ?? throw new InvalidOperationException("Database connection string is required");

builder.Services.AddPasswordHasher();
builder.Services.AddPersistenceContext(connectionString);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("UI", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("UI");
app.UseRouting();
app.MapControllers();

await app.RunAsync();
