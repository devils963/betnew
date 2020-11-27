using ConsoleApp1.Constants;
using ConsoleApp1.Models;
using ConsoleApp1.types;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleApp1.services
{
    class SpiDataController
    {
        private readonly ISpiDataService _spiDataService;

        public SpiDataController(ISpiDataService spiDataService)
        {
            _spiDataService = spiDataService;
        }

        public Game[] GetGamesToBetOnThisWeek()
        {
            var gamesToBetOnNextWeek = new List<Game>();
            var intervall = SpiProbabilityIntervall.spiProbabilityIntervall.Skip(6).Take(13);
            foreach (var probabilityIntervall in intervall)
            {
                double predictionAccuracy = _spiDataService.GetPredictionAccuracyOfIntervall(probabilityIntervall);
                double minimalBettingQuote = 1 / predictionAccuracy;

                SpiGameData[] gamesNextWeek = _spiDataService.GetGamesInNextWeekOfProbabilityIntervall(probabilityIntervall);

                Game[] bettableGame = AddMinimalBettingQuoteOnGames(gamesNextWeek, minimalBettingQuote);
                gamesToBetOnNextWeek.AddRange(bettableGame);
            }

            return gamesToBetOnNextWeek.ToArray();
        }

        private Game[] AddMinimalBettingQuoteOnGames(SpiGameData[] games, double minimalBettingQuote)
        {
            var gamesToReturn = new List<Game>(); 
            foreach (var game in games)
            {
                Game gameToReturn = MapToGame(game); 
                gameToReturn.minimalBettingOdd = minimalBettingQuote;
                gamesToReturn.Add(gameToReturn);
            }

            return gamesToReturn.ToArray();
        }

        private Game MapToGame(SpiGameData spiGame)
        {
            var gameToReturn = new Game();
            PropertyInfo[] properties = typeof(SpiGameData).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                property.SetValue(gameToReturn, property.GetValue(spiGame));
            }
            return gameToReturn;
        }
    }
}
