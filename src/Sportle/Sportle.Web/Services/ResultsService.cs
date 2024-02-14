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
            var top3 = Listify(result.RaceP1, result.RaceP2, result.RaceP3);
            var rest10 = Listify(result.RaceP4, result.RaceP5, result.RaceP6, result.RaceP7, result.RaceP8, result.RaceP9, result.RaceP10);

            var predictions = _dbContext.Predictions2024.Where(p => p.EventId == eventId);

            foreach (var prediction in predictions)
                await ProcessPrediction(prediction, result, firstSession, top3, rest10);

            await _dbContext.SaveChangesAsync();
        }

        public List<Guid> Listify(params Guid?[] items)
        {
            var result = new List<Guid>();
            foreach (var item in items)
                if (item is not null)
                    result.Add(item.Value);

            return result;
        }

        private async Task ProcessPrediction(EventPrediction2024 prediction, EventResult2024 result, DateTime firstSession, List<Guid> top3, List<Guid> rest10)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == prediction.UserId.ToString());

            ResetPoints(prediction);
            DetermineEarlyBonus(prediction, firstSession);
            DetermineSprintPoints(prediction, result);
            DetermineTop10Points(prediction,
                top3,
                rest10,
                prediction.RaceP1,
                prediction.RaceP2,
                prediction.RaceP3,
                prediction.RaceP4,
                prediction.RaceP5,
                prediction.RaceP6,
                prediction.RaceP7,
                prediction.RaceP8,
                prediction.RaceP9,
                prediction.RaceP10);
            DeterminePositionBonus(prediction, result);
        }

        private static void ResetPoints(EventPrediction2024 prediction)
        {
            prediction.Points = 0;
            prediction.EarlyBonus = 0;
            prediction.SprintBonus = 0;
            prediction.PodiumBonus = 0;
            prediction.PositionBonus = 0;
        }

        private static void DetermineEarlyBonus(EventPrediction2024 prediction, DateTime firstSession)
        {
            if (prediction.PredictedOn < firstSession)
                prediction.EarlyBonus = 3;
        }

        private static void DetermineSprintPoints(EventPrediction2024 prediction, EventResult2024 result)
        {
            var hasBonus = true;

            if (result.SprintPP is not null && prediction.SprintPP == result.SprintPP)
                prediction.Points += 1;
            else
                hasBonus = false;

            if (result.SprintP1 is not null && prediction.SprintP1 == result.SprintP1)
                prediction.Points += 1;
            else
                hasBonus = false;

            if (result.SprintFL is not null && prediction.SprintFL == result.SprintFL)
                prediction.Points += 1;
            else
                hasBonus = false;

            if (hasBonus)
                prediction.SprintBonus = 2;
        }

        private void DetermineTop10Points(EventPrediction2024 prediction, List<Guid> top3, List<Guid> rest10, params Guid?[] top10)
        {
            foreach (var driver in top10)
                if (driver is not null && (top3.Contains(driver.Value) || rest10.Contains(driver.Value)))
                    prediction.Points += 1;
        }

        private static void DeterminePositionBonus(EventPrediction2024 prediction, EventResult2024 result)
        {
            if (prediction.RacePP == result.RacePP)
                prediction.Points += 1;

            if (prediction.RaceP1 == result.RaceP1)
                prediction.PositionBonus += 1;

            if (prediction.RaceP2 == result.RaceP2)
                prediction.PositionBonus += 1;

            if (prediction.RaceP3 == result.RaceP3)
                prediction.PositionBonus += 1;

            if (prediction.RaceP4 == result.RaceP4)
                prediction.PositionBonus += .5;

            if (prediction.RaceP5 == result.RaceP5)
                prediction.PositionBonus += .5;

            if (prediction.RaceP6 == result.RaceP6)
                prediction.PositionBonus += .5;

            if (prediction.RaceP7 == result.RaceP7)
                prediction.PositionBonus += .5;

            if (prediction.RaceP8 == result.RaceP8)
                prediction.PositionBonus += .5;

            if (prediction.RaceP9 == result.RaceP9)
                prediction.PositionBonus += .5;

            if (prediction.RaceP10 == result.RaceP10)
                prediction.PositionBonus += .5;

            if (prediction.RaceFL == result.RaceFL)
                prediction.Points += 1;
        }
    }
}
