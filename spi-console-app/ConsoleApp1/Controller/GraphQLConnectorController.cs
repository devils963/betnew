using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using GraphQL;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace SpiFootballPrediction.Controller
{
    public class ResponseType
    {
        public String homeTeam { get; set; }
        public String awayTeam { get; set; }
    }

    class GraphQLConnectorController
    {
        public async Task<bool> SendGamesToServer(Game[] gamesToBetOn, string createSecret) {
            var graphQLClient = new GraphQLHttpClient("https://graph.cratory.de/graphql", new NewtonsoftJsonSerializer());
            foreach (var game in gamesToBetOn)
            {
                var mutation = @"
                    mutation createGame($league: String!, $homeTeam:String!,$awayTeam:String!,$homeProbability:Float!,$awayProbability:Float!,$homeIsWinning:Boolean!,$minimalBettingOdd:Float!,$date:String!, $createSecret: String!) {
                        createGame(
                            league: $league
                            homeTeam: $homeTeam
                            awayTeam: $awayTeam
                            probabilityHomeTeamWin: $homeProbability
                            probabilityAwayTeamWin: $awayProbability
                            homeTeamIsWinningSide: $homeIsWinning
                            minimalBettingOdd: $minimalBettingOdd
                            date: $date,
                            createSecret: $createSecret
                        ) {
                            homeTeam
                            awayTeam
                        }
                    }
                ";
                var homeSideIsWinning = game.prob1 > game.prob2;
                var queryRequest = new GraphQLRequest()
                {
                    Query = mutation,
                    Variables = new Dictionary<string, object>()
                {
                    {"league", game.league },
                    { "homeTeam", game.team1 },
                    { "awayTeam", game.team2 },
                    { "homeProbability", game.prob1 },
                    { "awayProbability", game.prob2 },
                    { "homeIsWinning", (game.prob1 > game.prob2) },
                    { "minimalBettingOdd", game.minimalBettingOdd },
                    { "date", game.date.ToString() },
                    {"createSecret", createSecret }
                }
                };

                var graphQLResponse = await graphQLClient.SendMutationAsync<ResponseType>(queryRequest);
            }

            return true;

            
        }
    }
}
