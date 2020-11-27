using ConsoleApp1.services;
using ConsoleApp1.Services;
using ConsoleApp1.View;
using Microsoft.Extensions.DependencyInjection;
using SpiFootballPrediction.Controller;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            var serviceProvider = new ServiceCollection()
            .AddSingleton<IOddsService, OddsService>()
            .AddSingleton<IDataMatcherService, DataMatcherService>()
            .AddSingleton<ISpiDataService, SpiDataService>()
            .BuildServiceProvider();

            var args = Environment.GetCommandLineArgs();

            if (args.Length < 3)
            {
                throw new Exception("Rapid-Api-Key or CreateSecret not defined");
            }

            string apiKey = args[1];
            string createSecret = args[2];

            var oddsController = new OddsController(serviceProvider.GetService<IOddsService>(), serviceProvider.GetService<IDataMatcherService>());
            var spiDataController = new SpiDataController(serviceProvider.GetService<ISpiDataService>());
            var graphQlController = new GraphQLConnectorController();

            var view = new GameView();

            var gamesToBetOnThisWeek = spiDataController.GetGamesToBetOnThisWeek();
            view.ShowAllGames(gamesToBetOnThisWeek);
            graphQlController.SendGamesToServer(gamesToBetOnThisWeek, createSecret).GetAwaiter().GetResult();  
        }
    }
}
