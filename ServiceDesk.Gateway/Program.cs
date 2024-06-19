using Microsoft.AspNetCore.Authentication.JwtBearer;
using OpenTelemetry.Trace;
using ServiceDesk.Gateway;
using ServiceDesk.Util;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<QueueService>();

builder
    .Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder
    .Services
    .AddOpenTelemetry()
    .WithTracing(trace =>
    {
        trace.AddAspNetCoreInstrumentation();
        trace.AddHttpClientInstrumentation(options =>
        {
            options.EnrichWithHttpResponseMessage = BaggageProcessEnricher.LoadBaggage;
        });

        trace.AddSource("Yarp.ReverseProxy");
        trace.AddBaggageProcessor();
    });

builder
    .Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://localhost:5402";
        options.Audience = "service-desk";
        options.RequireHttpsMetadata = false;
    });

builder
    .Services
    .AddAuthorization(options =>
    {
        options.AddPolicy("Authenticated", options => options.RequireAuthenticatedUser());
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();
app.MapReverseProxy();

await app.RunAsync();
