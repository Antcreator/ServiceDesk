using ServiceDesk.Data.Context;
using ServiceDesk.Tickets.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Database") 
    ?? throw new InvalidOperationException("Database connection string is required");

builder.Services.AddPersistenceContext(connectionString);
builder.Services.AddHttpClient<DocumentService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5401/api/Document");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapControllers();

app.Run();
