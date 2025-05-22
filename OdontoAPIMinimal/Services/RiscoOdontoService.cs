using Microsoft.ML.Data;
using Microsoft.ML;
using OdontoAPIMinimal.Models;

namespace OdontoAPIMinimal.Services
{
    public static class RiscoOdontoService
    {
        public static void RegisterRiscoOdontoEndpointService(this WebApplication app)
        {
            app.MapPost("/treinar-modelo", (List<PacienteOdontoModel> pacientes) =>
            {
                var mlContext = new MLContext();

                // Converte a lista para IDataView
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

                // Salva o modelo em disco
                mlContext.Model.Save(modelo, dadosTreinamento.Schema, "modelo_odonto.zip");

                return Results.Ok("✅ Modelo treinado e salvo com sucesso!");
            });
            app.MapPost("/prever", (List<PacienteOdontoModel> pacientes) =>

            {
                var mlContext = new MLContext();
                var modelo = mlContext.Model.Load("modelo_odonto.zip", out var schema);

                var predictionEngine = mlContext.Model.CreatePredictionEngine<PacienteOdontoModel, RiscoPrediction>(modelo);

                var resultados = pacientes.Select(paciente => predictionEngine.Predict(paciente)).ToList();

                return Results.Ok(resultados);
            });
        }
    }
}
