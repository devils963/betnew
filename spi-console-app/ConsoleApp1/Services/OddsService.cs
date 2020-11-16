using ConsoleApp1.types;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ConsoleApp1.Services
{
    public interface IOddsService
    {
        FootballApiFixture[] GetGamesOfDate(DateTime date, string apiKey);

        Odd GetOddOfGame(FootballApiFixture game, bool homeIsWinningSide, double minimal_betting_odd, string apiKey);

    }
    public class OddsService : IOddsService
    {
        private readonly string baseUrl = "https://api-football-v1.p.rapidapi.com/v2/";

        public FootballApiFixture[] GetGamesOfDate(DateTime date, string apiKey)
        {
            var gamesOfDate = new List<FootballApiFixture>();
            var formattedDate = string.Concat(date.Year, "-", date.Month, "-", date.Day);
            var url = baseUrl + "fixtures/date/" + formattedDate;
            var jsonData = MakeRequest(url, apiKey);
            
            var fixtures = jsonData.SelectToken("api.fixtures");
            foreach (var fixture in fixtures)
            {
                var fixtureId = fixture.SelectToken("fixture_id").Value<int>();
                var homeTeamName = fixture.SelectToken("homeTeam.team_name").Value<string>();
                var awayTeamName = fixture.SelectToken("awayTeam.team_name").Value<string>();

                var fixtureToAdd = new FootballApiFixture(fixtureId, homeTeamName, awayTeamName);

                gamesOfDate.Add(fixtureToAdd);
            }
            return gamesOfDate.ToArray();
        }

        public Odd GetOddOfGame(FootballApiFixture game, bool homeIsWinningSide, double minimal_betting_odd, string apiKey)
        {
            var url = "https://api-football-v1.p.rapidapi.com/v2/odds/fixture/" + game.id;
            Odd bestOdd = null;

            var jsonData = MakeRequest(url, apiKey);

            var bookmakers = jsonData.SelectToken("api.odds[0].bookmakers");
            if (bookmakers == null) throw new Exception("no bookmakers found on game");

            foreach (var bookmaker in bookmakers)
            {
                var betTypes = bookmaker.SelectToken("bets");
                var headToHeadOdd = getHeadToHeadOdd(betTypes);
                var winnersOdd = GetWinnersOdd(headToHeadOdd, homeIsWinningSide);
               
                if (winnersOdd > minimal_betting_odd && IsBetterThanExistingBestOdd(winnersOdd, bestOdd))
                {
                    bestOdd = ComputeOddObject(bookmaker.SelectToken("bookmaker_name").ToString(), winnersOdd);
                }    
            }
            return bestOdd ?? throw new Exception("no odd found");
        }

        private JToken getHeadToHeadOdd(JToken betTypes)
        {
            foreach (var bet in betTypes)
            {
                if (bet.SelectToken("label_name").Value<string>() == "Match Winner")
                {
                    return bet;
                }
            }
            throw new Exception("no head to head odd");
        }

        private double GetWinnersOdd(JToken headToHeadOdd, bool homeIsWinningSide)
        {
            var bettingValues = headToHeadOdd.SelectToken("values");
            foreach (var bettingValue in bettingValues)
            {
                var odd = bettingValue.SelectToken("odd").Value<float>();
                if ((homeIsWinningSide && bettingValue.SelectToken("value").ToString() == "Home") || 
                    (!homeIsWinningSide && bettingValue.SelectToken("value").ToString() == "Away"))
                {
                    return odd;
                }
            }
            throw new Exception("could not get Winners odd");
        }

        private JObject MakeRequest(string url, string apiKey)
        {
            if (!RequestLimitExceeded())
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("X-RapidAPI-Key", apiKey);
                client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "api-football-v1.p.rapidapi.com");
                HttpResponseMessage response = client.GetAsync(url).Result;
                SetRemainingRequests(response.Headers);
                return JObject.Parse(response.Content.ReadAsStringAsync().Result);
            }
            throw new Exception("request limit exceeded");
        }

        private bool RequestLimitExceeded()
        {
            var remainingRequests = int.Parse(System.IO.File.ReadAllText(@"..\..\..\Constants\RequestLimit.txt"));
            if (remainingRequests <= 1)
            {
                return true;
            }
            return false;
        }

        private void SetRemainingRequests(HttpResponseHeaders headers)
        {
            var remainingRequests = headers.GetValues("x-ratelimit-requests-remaining");
            System.IO.File.WriteAllLines(@"..\..\..\Constants\RequestLimit.txt", remainingRequests);
        }

        private bool IsBetterThanExistingBestOdd(double currentOdd, Odd bestExistingOdd)
        {
            if (bestExistingOdd == null)
            {
                return true;
            }
            if (currentOdd > bestExistingOdd.bestOdd)
            {
                return true;
            }
            return false;
        }

        private Odd ComputeOddObject(string bookMakerName, double bestOdd)
        {
            var odd = new Odd();
            odd.bestOdd = bestOdd;
            odd.bookmakerName = bookMakerName;
            return odd;
        }
    }
}
