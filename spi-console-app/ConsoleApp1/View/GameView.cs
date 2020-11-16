using ConsoleApp1.Models;
using System;

namespace ConsoleApp1.View
{
    class GameView
    {
        public void ShowAllGames(Game[] games)
        {
            foreach (var game in games)
            {
                var winner = game.prob1 > game.prob2 ? game.team1 : game.team2;
                Console.WriteLine(game.team1 + " vs. " + game.team2);
                Console.WriteLine("predicted Winner is " + winner);
                Console.WriteLine("minimal betting Odd for betting on " + winner + " is : " + string.Format("{0:N2}", game.minimalBettingOdd));
                if (game.odd != null)
                {
                    Console.WriteLine("game that matched: " + game.odd.matchedHomeTeam + " vs. " + game.odd.matchedAwayTeam);
                    Console.WriteLine("best odd: " + game.odd.bestOdd + " at bookmaker ", game.odd.bookmakerName);
                } else
                {
                    Console.WriteLine("Couldnt Get Odd of Game");
                }
                Console.WriteLine("-------------------------------------------------------------------------------------");
            }
        }

        public int GetChosenMenuOption()
        {
            while (true)
            {
                Console.WriteLine("Choose a option: ");
                Console.WriteLine("<1> Get all games to bet on this week");
                Console.WriteLine("<2> Get minimal betting odd of specific game");
                var key = Console.ReadKey().KeyChar.ToString();
                if (int.TryParse(key, out int testResult))
                {
                    if (testResult == 1 || testResult == 2)
                    {
                        return testResult;
                    }
                }
                Console.WriteLine("Please enter a valid option");
            }
        }

        public void ShowGame(Game game)
        {
            var winner = game.prob1 > game.prob2 ? game.team1 : game.team2;
            Console.WriteLine(game.team1 + " vs. " + game.team2);
            Console.WriteLine("predicted Winner is " + winner);
            Console.WriteLine("minimal betting Odd for betting on " + winner + " is : " + string.Format("{0:N2}", game.minimalBettingOdd));
        }

        public int GetGameId()
        {
            while (true)
            {
                Console.WriteLine("For which game do you want to know the minimal betting odd?");
                var id = Console.ReadLine().ToString();
                if (int.TryParse(id, out int testResult))
                {
                    return testResult;
                }
                Console.WriteLine("Enter a valid Id");
            }
        }
    }
}
