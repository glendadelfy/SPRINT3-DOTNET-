using Microsoft.ML.Data;
using Microsoft.ML;
using OdontoAPIMinimal.Models;

namespace OdontoAPIMinimal.Middelewares.Endpoints
{
    public static class RiscoOdontoEndpoint
    {
        public static void RegisterRiscoOdontoEndpoint(this WebApplication app)
        {
            var riscoOdontoGroup = app.MapGroup("/risco-odonto");

            riscoOdontoGroup.MapPost("/treinar-modelo", (List<PacienteOdontoModel> pacientes) =>
            {
                var mlContext = new MLContext();
                var dadosTreinamento = mlContext.Data.LoadFromEnumerable(pacientes);

                var pipeline = mlContext.Transforms.CopyColumns("Label", "RiscoAlto")
                    .Append(mlContext.Transforms.Conversion.ConvertType(
                        new[]
                        {
                        new InputOutputColumnPair("HistoricoCaries"),
                        new InputOutputColumnPair("DoencaGengival"),
                        new InputOutputColumnPair("Fumante"),
                        new InputOutputColumnPair("ConsomeAcucarFrequente")
                        },
                        outputKind: DataKind.Single))
                    .Append(mlContext.Transforms.Concatenate("Features",
                        nameof(PacienteOdontoModel.Idade),
                        nameof(PacienteOdontoModel.FrequenciaEscovacao),
                        nameof(PacienteOdontoModel.FrequenciaVisitasAno),
                        "HistoricoCaries",
                        "DoencaGengival",
                        "Fumante",
                        "ConsomeAcucarFrequente"))
                    .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression());

                var modelo = pipeline.Fit(dadosTreinamento);
                mlContext.Model.Save(modelo, dadosTreinamento.Schema, "modelo_odonto.zip");

                return Results.Ok("✅ Modelo treinado e salvo com sucesso!");
            })
            .WithSummary("Treina e salva o modelo odontológico")
            .WithDescription("Recebe dados dos pacientes e treina um modelo de risco odontológico.");

            riscoOdontoGroup.MapPost("/prever", (List<PacienteOdontoModel> pacientes) =>
            {
                var mlContext = new MLContext();
                var modelo = mlContext.Model.Load("modelo_odonto.zip", out var schema);

                var predictionEngine = mlContext.Model.CreatePredictionEngine<PacienteOdontoModel, RiscoPrediction>(modelo);
                var resultados = pacientes.Select(paciente => predictionEngine.Predict(paciente)).ToList();

                return Results.Ok(resultados);
            })
            .WithSummary("Realiza previsões com o modelo odontológico")
            .WithDescription("Usa o modelo treinado para prever o risco odontológico dos pacientes.");
        }
    }
}
