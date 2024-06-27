using ServiceDesk.Data.Context;
using ServiceDesk.Util.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPasswordHasher();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Database")
    ?? throw new ArgumentNullException("Database connection string is required");

builder.Services.AddPersistenceContext(connectionString);
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
