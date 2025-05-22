using Microsoft.ML.Data;

namespace OdontoAPIMinimal.Models
{
    public class RiscoPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool PredictedLabel { get; set; }
        public float Probability { get; set; }
        public float Score { get; set; }

    }
}
