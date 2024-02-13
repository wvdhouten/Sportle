using Microsoft.EntityFrameworkCore;
using Sportle.Web.Data;
using Sportle.Web.Models.Formula1;

namespace Sportle.Web.Services
{
    public class ResultsService
    {
        private readonly SportleDbContext _dbContext;

        public ResultsService(SportleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ProcessResult(Guid eventId)
        {
            var result = await _dbContext.Results2024.FirstOrDefaultAsync(r => r.EventId == eventId) ?? throw new Exception("Result Not Found.");

            var firstSession = (await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == eventId))?.Sessions.OrderBy(s => s.Start).FirstOrDefault()?.Start ?? default;
            var top3 = new List<Guid>
            {
                result.RaceP1.Value,
                result.RaceP2.Value,
                result.RaceP3.Value,
            };
            var rest10 = new List<Guid>
            {
                result.RaceP4.Value,
                result.RaceP5.Value,
                result.RaceP6.Value,
                result.RaceP7.Value,
                result.RaceP8.Value,
                result.RaceP9.Value,
                result.RaceP10.Value
            };

            var predictions = _dbContext.Predictions2024.Where(p => p.EventId == eventId);

            foreach (var prediction in predictions)
            {
                await ProcessPrediction(prediction, result, firstSession, top3, rest10);
            }
        }

        private async Task ProcessPrediction(EventPrediction2024 prediction, EventResult2024 result, DateTime firstSession, List<Guid> top3, List<Guid> rest10)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == prediction.UserId.ToString());

            prediction.Points = 0;
            prediction.EarlyBonus = 0;
            prediction.SprintBonus = 0;
            prediction.PodiumBonus = 0;
            prediction.PositionBonus = 0;

            if (prediction.PredictedOn < firstSession)
                prediction.EarlyBonus = 3;

            if (result.SprintPP is not null && prediction.SprintPP == result.SprintPP)
                prediction.Points += 1;
            if (result.SprintP1 is not null && prediction.SprintP1 == result.SprintP1)
                prediction.Points += 1;
            if (result.SprintFL is not null && prediction.SprintFL == result.SprintFL)
                prediction.Points += 1;

            // If there are 3 points, that must mean the prediction qualifies for a sprint-bonus.
            if (prediction.Points == 3)
                prediction.SprintBonus = 2;

            if (prediction.RacePP == result.RacePP)
                prediction.Points += 1;

            if (top3.Contains(prediction.RaceP1.Value) || rest10.Contains(prediction.RaceP1.Value))
                prediction.Points += 1;

            if (prediction.RaceP1 == result.RaceP1)
                prediction.PositionBonus += 1;

            if (top3.Contains(prediction.RaceP2.Value) || rest10.Contains(prediction.RaceP2.Value))
                prediction.Points += 1;

            if (prediction.RaceP2 == result.RaceP2)
                prediction.PositionBonus += 1;

            if (top3.Contains(prediction.RaceP3.Value) || rest10.Contains(prediction.RaceP3.Value))
                prediction.Points += 1;

            if (top3.Contains(prediction.RaceP1.Value) && top3.Contains(prediction.RaceP2.Value) && top3.Contains(prediction.RaceP3.Value))
                prediction.PodiumBonus += 1;

            if (top3.Contains(prediction.RaceP1.Value) || rest10.Contains(prediction.RaceP1.Value))
                prediction.Points += 1;

            if (prediction.RaceP3 == result.RaceP3)
                prediction.PositionBonus += 1;

            if (top3.Contains(prediction.RaceP4.Value) || rest10.Contains(prediction.RaceP4.Value))
                prediction.Points += 1;

            if (prediction.RaceP4 == result.RaceP4)
                prediction.PositionBonus += .5;

            if (top3.Contains(prediction.RaceP5.Value) || rest10.Contains(prediction.RaceP5.Value))
                prediction.Points += 1;

            if (prediction.RaceP5 == result.RaceP5)
                prediction.PositionBonus += .5;

            if (top3.Contains(prediction.RaceP6.Value) || rest10.Contains(prediction.RaceP6.Value))
                prediction.Points += 1;

            if (prediction.RaceP6 == result.RaceP6)
                prediction.PositionBonus += .5;

            if (top3.Contains(prediction.RaceP7.Value) || rest10.Contains(prediction.RaceP7.Value))
                prediction.Points += 1;

            if (prediction.RaceP7 == result.RaceP7)
                prediction.PositionBonus += .5;

            if (top3.Contains(prediction.RaceP8.Value) || rest10.Contains(prediction.RaceP8.Value))
                prediction.Points += 1;

            if (prediction.RaceP8 == result.RaceP8)
                prediction.PositionBonus += .5;

            if (top3.Contains(prediction.RaceP9.Value) || rest10.Contains(prediction.RaceP9.Value))
                prediction.Points += 1;

            if (prediction.RaceP9 == result.RaceP9)
                prediction.PositionBonus += .5;

            if (top3.Contains(prediction.RaceP10.Value) || rest10.Contains(prediction.RaceP10.Value))
                prediction.Points += 1;

            if (prediction.RaceP10 == result.RaceP10)
                prediction.PositionBonus += .5;

            if (prediction.RaceFL == result.RaceFL)
                prediction.Points += 1;
        }
    }
}
