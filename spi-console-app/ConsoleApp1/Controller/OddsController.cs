using ConsoleApp1.Models;
using ConsoleApp1.Services;
using ConsoleApp1.types;
using System;

namespace ConsoleApp1.services
{
    class OddsController
    {
        private readonly IOddsService _oddsService;
        private readonly IDataMatcherService _dataMatcherService;
        public OddsController(IOddsService oddsService, IDataMatcherService dataMatcherService)
        {
            _oddsService = oddsService;
            _dataMatcherService = dataMatcherService;
        }
        public Odd GetBestOddOfGame(Game game, string apiKey) 
        {
            FootballApiFixture[] gamesOfDate = _oddsService.GetGamesOfDate(DateTime.Parse(game.date), apiKey);
            FootballApiFixture gameDerivedFromName = _dataMatcherService.DeriveGameFromTeamNames(gamesOfDate, game);
            bool homeSideWinning = game.prob1 > game.prob2;
            var odd = _oddsService.GetOddOfGame(gameDerivedFromName, homeSideWinning, game.minimalBettingOdd, apiKey);
            odd.matchedAwayTeam = gameDerivedFromName.homeTeam;
            odd.matchedHomeTeam = gameDerivedFromName.awayTeam;
            return odd;
        }
    }
}
