# ObservabilityDemo

🚀 Demonstração prática de **Observabilidade em aplicações .NET**, com **OpenTelemetry**, **Jaeger** e **Prometheus**, tudo funcionando localmente com o mínimo de configuração.

---

## 📌 Objetivo

Este projeto mostra como instrumentar uma aplicação ASP.NET com OpenTelemetry para coletar **traces distribuídos** e **métricas**, enviando esses dados para ferramentas como Jaeger e Prometheus via **OpenTelemetry Collector (otelcol)**.

---

## 🧰 Tecnologias e Ferramentas

- [.NET](https://dotnet.microsoft.com/)
- [OpenTelemetry .NET SDK](https://opentelemetry.io/docs/instrumentation/net/)
- [OpenTelemetry Collector](https://opentelemetry.io/docs/collector/)
- [Jaeger](https://www.jaegertracing.io/)
- [Prometheus (para métricas)](https://prometheus.io/)

---

## 🧱 Estrutura

- `ObservabilityExtensions.cs`: extensão para configurar o OpenTelemetry na aplicação.
- `otelcol.yaml`: configuração do Collector.
- `bin/`: diretório com os binários compactados do Jaeger e do Collector.
- `ObservabilityDemo.sln`: solução principal.

---

## ▶️ Como rodar localmente

### 1. Clone o repositório

```bash
git clone https://github.com/gustavolim/ObservabilityDemo.git
cd ObservabilityDemo


2. Extraia os binários
Extraia o conteúdo de Observability.7z (ou semelhante) para um diretório como bin/.

3. Inicie o OpenTelemetry Collector
bash
Copiar
Editar
cd bin/otelcol
./otelcol-contrib --config ../../otelcol.yaml
⚠️ Pode variar conforme o nome/pasta do executável e sistema operacional.

4. Rode a aplicação .NET
bash
Copiar
Editar
dotnet run --project src/SuaAplicacao.csproj
5. Acesse os dados
Jaeger UI: http://localhost:16686

Prometheus: http://localhost:8889/metrics

⚙️ Configurações observáveis
Configuradas via appsettings.json ou variáveis de ambiente:

json
Copiar
Editar
"Observability": {
  "ServiceName": "MyApp",
  "Environment": "dev",
  "OtlpEndpoint": "http://localhost:4317"
}
