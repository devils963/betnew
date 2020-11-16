using ConsoleApp1.types;
using System;
using System.Linq;

namespace ConsoleApp1.services
{
    public interface IDataMatcherService
    {
        FootballApiFixture DeriveGameFromTeamNames(FootballApiFixture[] gamesOfDate, SpiGameData game);
    }
    class DataMatcherService : IDataMatcherService
    {
        public FootballApiFixture DeriveGameFromTeamNames(FootballApiFixture[] gamesOfDate, SpiGameData game)
        {
            int bestLevenshteinDistance = GetWorstLevenshteinDistance(gamesOfDate, game);
            FootballApiFixture bestMatchedGame = null;
            foreach (var apiGame in gamesOfDate)
            {
                var homeTeamLevenshteinScore = CalculateLevenshteinDistance(game.team1, apiGame.homeTeam);
                var awayTeamLevenshteinScore = CalculateLevenshteinDistance(game.team2, apiGame.awayTeam);
                if (homeTeamLevenshteinScore < bestLevenshteinDistance || awayTeamLevenshteinScore < bestLevenshteinDistance)
                {
                    bestLevenshteinDistance = homeTeamLevenshteinScore < awayTeamLevenshteinScore ? homeTeamLevenshteinScore : awayTeamLevenshteinScore;
                    bestMatchedGame = apiGame;
                }
            }
            return bestMatchedGame ?? throw new Exception("no game match found");
        }

        private int GetWorstLevenshteinDistance(FootballApiFixture[] gamesOfDate, SpiGameData game)
        {
            var longestHomeTeamInGamesOfDate = gamesOfDate.OrderBy(n => n.homeTeam.Length).LastOrDefault().homeTeam;
            var longestAwayTeamInGamesOfDate = gamesOfDate.OrderBy(n => n.awayTeam.Length).LastOrDefault().awayTeam;
            int worstLevenshteinDistance = Math.Max(Math.Max(game.team1.Length, game.team2.Length),
                                                    Math.Max(longestHomeTeamInGamesOfDate.Length, longestAwayTeamInGamesOfDate.Length));
            return worstLevenshteinDistance;
        }

        private int CalculateLevenshteinDistance(string a, string b)
        {
            if (string.IsNullOrEmpty(a) && string.IsNullOrEmpty(b))
            {
                return 0;
            }
            if (string.IsNullOrEmpty(a))
            {
                return b.Length;
            }
            if (string.IsNullOrEmpty(b))
            {
                return a.Length;
            }
            int lengthA = a.Length;
            int lengthB = b.Length;
            var distances = new int[lengthA + 1, lengthB + 1];
            for (int i = 0; i <= lengthA; distances[i, 0] = i++) ;
            for (int j = 0; j <= lengthB; distances[0, j] = j++) ;

            for (int i = 1; i <= lengthA; i++)
                for (int j = 1; j <= lengthB; j++)
                {
                    int cost = b[j - 1] == a[i - 1] ? 0 : 1;
                    distances[i, j] = Math.Min
                        (
                        Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                        distances[i - 1, j - 1] + cost
                        );
                }
            return distances[lengthA, lengthB];
        }
    }
}
