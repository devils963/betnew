import { gql } from 'apollo-server-express';

export const typeDefs = gql`
  type Query {
    games: [Game!]!
    game(id: ID!): Game!
    gamesThisWeek: [Game!]!
  }

  type Game {
    id: ID!
    league: String!
    homeTeam: String!
    awayTeam: String!
    probabilityHomeTeamWin: Float!
    probabilityAwayTeamWin: Float!
    homeTeamIsWinningSide: Boolean!
    minimalBettingOdd: Float!
    date: String!
  }

  type Mutation {
    createGame(
      league: String!
      homeTeam: String!
      awayTeam: String!
      probabilityHomeTeamWin: Float!
      probabilityAwayTeamWin: Float!
      homeTeamIsWinningSide: Boolean!
      minimalBettingOdd: Float!
      date: String!,
      createSecret: String!
    ): Game!
  }
`;
