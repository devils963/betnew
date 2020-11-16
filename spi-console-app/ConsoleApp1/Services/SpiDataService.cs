using ConsoleApp1.Constants;
using ConsoleApp1.types;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;

namespace ConsoleApp1.services
{
    public interface ISpiDataService
    {
        SpiGameData GetGameById(int id);

        double GetPredictionAccuracyOfIntervall(ProbabilityIntervall intervall);

        SpiGameData[] GetGamesInNextWeekOfProbabilityIntervall(ProbabilityIntervall intervall);
    }
    class SpiDataService : ISpiDataService
    {
        readonly SpiGameData[] gamesToProcess;

        public SpiDataService()
        {
            gamesToProcess = FetchSpiFile();
        }

        public SpiGameData GetGameById(int id)
        {
            return gamesToProcess[id];
        }

        public double GetPredictionAccuracyOfIntervall(ProbabilityIntervall intervall)
        {
            double correctPredictedGames = 0;
            var finishedGamesInIntervall = GetGamesInProbabilityIntervall(intervall, true);
            double allGames = finishedGamesInIntervall.Length;
            foreach (var game in finishedGamesInIntervall)
            {
                if (game.score1 > game.score2 && game.prob1 > game.prob2 ||
                    game.score2 > game.score1 && game.prob2 > game.prob1)
                {
                    correctPredictedGames++;
                }
            }
            double predictionAccuracy = correctPredictedGames / allGames;
            return predictionAccuracy;
        }

        public SpiGameData[] GetGamesInNextWeekOfProbabilityIntervall(ProbabilityIntervall intervall)
        {
            var gamesNotFinishedInIntervall = GetGamesInProbabilityIntervall(intervall, false);
            return GetGamesInNextWeek(gamesNotFinishedInIntervall);
        }

        private SpiGameData[] FetchSpiFile()
        {
            var spiFileStream = GetSpiFileStream();
            using (var reader = new StreamReader(spiFileStream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<SpiGameData>();
                return records.ToArray();
            }
        }

        private SpiGameData[] GetGamesInProbabilityIntervall(ProbabilityIntervall intervall, bool finished)
        {
            var gamesInProbabilityIntervall = new List<SpiGameData>();
            foreach (var game in gamesToProcess)
            {
                bool homeTeamInProbabilityRange = game.prob1 >= intervall.minimumProbabilityBorder && game.prob1 <= intervall.maximumProbabilityBorder;
                bool awayTeamInProbabilityRange = game.prob2 >= intervall.minimumProbabilityBorder && game.prob2 <= intervall.maximumProbabilityBorder;

                bool finishedCriteria = game.score1 != null && game.score2 != null;
                bool notFinishedCriteria = game.score1 == null && game.score2 == null;
                bool filterOnFinished = finished ? finishedCriteria : notFinishedCriteria;

                if ((homeTeamInProbabilityRange || awayTeamInProbabilityRange) && filterOnFinished)
                {
                    gamesInProbabilityIntervall.Add(game);
                }
            }
            return gamesInProbabilityIntervall.ToArray();
        }

        private SpiGameData[] GetGamesInNextWeek(SpiGameData[] games)
        {
            var gamesNextWeek = new List<SpiGameData>();

            foreach (var game in games)
            {
                var gameDate = DateTime.Parse(game.date);

                if (gameDate > DateTime.Now && gameDate < DateTime.Now.AddDays(7))
                {
                    gamesNextWeek.Add(game);
                }
            }

            return gamesNextWeek.ToArray();
        }

        private Stream GetSpiFileStream()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://projects.fivethirtyeight.com/soccer-api/club/spi_matches.csv");
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            return resp.GetResponseStream();
        }
    }
}
