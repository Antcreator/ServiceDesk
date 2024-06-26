using System.Text.Json.Serialization;
using ServiceDesk.Data.Context;
using ServiceDesk.Tickets.Service;
using ServiceDesk.Util;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Database") 
    ?? throw new InvalidOperationException("Database connection string is required");

builder.Services.AddPersistenceContext(connectionString);
builder.Services.AddHttpClient<DocumentService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5401/api/Document/");
});
builder.Services.AddSingleton<QueueService>();
builder.Services.AddControllers();
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
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
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

app.Run();
