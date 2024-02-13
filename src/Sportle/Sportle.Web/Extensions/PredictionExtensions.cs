using Sportle.Web.Models.Formula1;

namespace Sportle.Web.Extensions
{
    public static class PredictionExtensions
    {
        public static double GetTotalPoints(this EventPrediction2024? prediction)
        {
            if (prediction == null)
                return 0;

            return prediction.Points + prediction.EarlyBonus + prediction.SprintBonus + prediction.PodiumBonus + prediction.PositionBonus;
        }
    }
}
