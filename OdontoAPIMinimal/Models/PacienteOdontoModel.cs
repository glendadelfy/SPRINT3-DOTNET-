using Microsoft.ML.Data;

namespace OdontoAPIMinimal.Models
{
    public class PacienteOdontoModel
    {
        public float Idade { get; set; }
        public float FrequenciaEscovacao { get; set; }
        public float FrequenciaVisitasAno { get; set; }
        public bool HistoricoCaries { get; set; }
        public bool DoencaGengival { get; set; }
        public bool Fumante { get; set; }
        public bool ConsomeAcucarFrequente { get; set; }

        [ColumnName("RiscoAlto")]
        public bool RiscoAlto { get; set; }
    }
}
