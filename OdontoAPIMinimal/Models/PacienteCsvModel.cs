using Microsoft.ML.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace OdontoAPIMinimal.Models
{
    public class PacienteCsvModel
    {
        [LoadColumn(0)] public float Idade { get; set; }
        [LoadColumn(1)] public float FrequenciaEscovacao { get; set; }
        [LoadColumn(2)] public float FrequenciaVisitasAno { get; set; }
        [LoadColumn(3)] public bool HistoricoCaries { get; set; }
        [LoadColumn(4)] public bool DoencaGengival { get; set; }
        [LoadColumn(5)] public bool Fumante { get; set; }
        [LoadColumn(6)] public bool ConsomeAcucarFrequente { get; set; }

        [LoadColumn(7), ColumnName("RiscoAlto")]
        public bool RiscoAlto { get; set; }
    }
}