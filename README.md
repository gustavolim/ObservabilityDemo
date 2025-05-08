MyApp Observability Stack 📊🚀
Um projeto completo de monitoramento distribuído usando OpenTelemetry e Jaeger para aplicações .NET.

Tecnologias Utilizadas
🖥️ .NET 7+ (Application)

📡 OpenTelemetry (Instrumentation)

🔍 Jaeger (Tracing)

📊 Prometheus (Metrics - Opcional)

🐳 Docker (Para Jaeger - Opcional)

📋 Pré-requisitos
.NET 7+ SDK

Jaeger All-in-One (ou Docker)

OpenTelemetry Collector

⚙️ Configuração
1️⃣ AppSettings.json
json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "OpenTelemetry": "Debug"
    }
  },
  "Observability": {
    "ServiceName": "MyApp.Api",
    "Environment": "dev",
    "OtlpEndpoint": "http://localhost:4317"
  }
}
2️⃣ OpenTelemetry Collector (otel-final.yaml)
yaml
receivers:
  otlp:
    protocols:
      grpc:
        endpoint: "0.0.0.0:4317"

exporters:
  otlp/jaeger:
    endpoint: "localhost:14250"
    tls:
      insecure: true

service:
  pipelines:
    traces:
      receivers: [otlp]
      exporters: [otlp/jaeger]
3️⃣ Código de Instrumentação (ObservabilityExtensions.cs)
csharp
services.AddOpenTelemetry()
    .WithTracing(builder => 
    {
        builder
            .SetResourceBuilder(ResourceBuilder.CreateDefault()
                .AddService("MyApp.Api"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter(o => 
            {
                o.Endpoint = new Uri("http://localhost:4317");
                o.Protocol = OtlpExportProtocol.Grpc;
            });
    });
🚀 Como Executar
1. Inicie o Jaeger
powershell
.\jaeger-all-in-one.exe --collector.grpc.tls.enabled=false
(Ou via Docker: docker run -p 16686:16686 -p 14250:14250 jaegertracing/all-in-one)

2. Inicie o OpenTelemetry Collector
powershell
.\otelcol-contrib.exe --config .\otel-final.yaml
3. Execute sua aplicação .NET
powershell
dotnet run
4. Acesse o Jaeger UI
🌐 http://localhost:16686

🔍 Troubleshooting
Se não aparecer traces:

Verifique logs:

powershell
$env:OTEL_LOG_LEVEL="DEBUG"
.\otelcol-contrib.exe --config .\otel-final.yaml
Teste manualmente:

powershell
curl http://localhost:4317/v1/traces
Confira o SDK:

csharp
.AddConsoleExporter() // Adicione para debug
📂 Estrutura do Projeto
MyApp/
├── src/
│   ├── MyApp.Api/          # Aplicação principal
│   ├── MyCompany.Observability/  # SDK OpenTelemetry
├── observability/
│   ├── otel-final.yaml     # Config Collector
│   ├── jaeger-all-in-one.exe


🎯 Próximos Passos
Adicionar métricas com Prometheus

Configurar alertas no Jaeger

Implementar logging estruturado

✏️ Contribuições são bem-vindas!
🔗 Documentação: OpenTelemetry | Jaeger
