using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Net.Http.Headers;
using OpenTelemetry;
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
            options.EnrichWithHttpResponseMessage = (activity, response) =>
            {
                if (response.Headers.Contains(HeaderNames.Baggage))
                {
                    var outbox = response.Headers
                        .GetValues(HeaderNames.Baggage)
                        .First()
                        .Split("=")[1];

                    activity.AddBaggage("outbox", outbox);
                }
            };
        });

        trace.AddSource("Yarp.ReverseProxy");

        trace.AddProcessor(new BaggageFilter());
        trace.AddProcessor(services =>
        {
            var queue = services.GetRequiredService<QueueService>();
            var exporter = new BaggageExporter(queue);

            return new SimpleActivityExportProcessor(exporter);
        });
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
