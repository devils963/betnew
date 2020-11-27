import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Game } from '../types';
import { Apollo } from 'apollo-angular';
import { map } from 'rxjs/operators';
import gql from 'graphql-tag';
import cloneDeep from 'lodash.clonedeep';

const GAMES_QUERY = gql`
  query {
    games {
      league
      homeTeam
      awayTeam
      minimalBettingOdd
      homeTeamIsWinningSide
      date
    }
  }
`;

const GAMES_THIS_WEEK_QUERY = gql`
  query {
    gamesThisWeek {
      league
      homeTeam
      awayTeam
      homeTeamIsWinningSide
      minimalBettingOdd
      date
    }
  }
`;

@Injectable({
  providedIn: 'root',
})
export class GameService {
  constructor(private apollo: Apollo) {}

  getGames(): Observable<Game[]> {
    return this.apollo
      .watchQuery<{ games: Game[] }>({
        query: GAMES_QUERY,
        variables: {},
      })
      .valueChanges.pipe(map((apolloResult) => apolloResult.data.games));
  }

  getGamesThisWeek(): Observable<Game[]> {
    return this.apollo
      .watchQuery<{ gamesThisWeek: Game[] }>({
        query: GAMES_THIS_WEEK_QUERY,
        variables: {},
      })
      .valueChanges.pipe(
        map((apolloResults) => apolloResults.data.gamesThisWeek),
        map((games) => cloneDeep(games)),
        map((games) => {
          for (const game of games) {
            game.minimalBettingOdd = Math.ceil(game.minimalBettingOdd * 100) / 100;
          }
          return games;
        })
      );
  }
}
