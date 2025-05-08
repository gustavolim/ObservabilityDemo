using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics; // Adicione esta linha

namespace MyCompany.Observability
{
    public static class ObservabilityExtensions
    {
        public static IServiceCollection AddMyCompanyObservability(this IServiceCollection services, IConfiguration config)
        {
            var serviceName = config["Observability:ServiceName"] ?? "myapp";
            var environment = config["Observability:Environment"] ?? "dev";
            var otlpEndpoint = config["Observability:OtlpEndpoint"] ?? "http://localhost:4317";

            var resourceBuilder = ResourceBuilder.CreateDefault()
                .AddService(serviceName)
                .AddAttributes(new[]
                {
                    new KeyValuePair<string, object>("environment", environment),
                    new KeyValuePair<string, object>("version", "1.0.0")
                });

            services.AddOpenTelemetry()
                .WithTracing(builder =>
                {
                    builder
                        .SetResourceBuilder(resourceBuilder)
                        .AddAspNetCoreInstrumentation(options =>
                        {
                            options.Filter = httpContext => 
                                !httpContext.Request.Path.StartsWithSegments("/healthcheck");
                            options.EnrichWithHttpRequest = (activity, request) =>
                            {
                                activity.SetTag("http.client_ip", request.HttpContext.Connection.RemoteIpAddress);
                            };
                        })
                        .AddHttpClientInstrumentation()
                        .AddConsoleExporter() // Agora funcionará após adicionar o pacote
                        .AddOtlpExporter(o =>
                        {
                            o.Endpoint = new Uri(otlpEndpoint);
                            o.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                        });
                });

            // Adicione este trecho para logar a configuração
            Console.WriteLine($"OpenTelemetry Config:");
            Console.WriteLine($"- Service: {serviceName}");
            Console.WriteLine($"- Environment: {environment}");
            Console.WriteLine($"- OTLP Endpoint: {otlpEndpoint}");

            return services;
        }
    }
}