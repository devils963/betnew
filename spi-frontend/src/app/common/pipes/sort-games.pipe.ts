import { formatDate } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';
import { Game, GamesSortKeys } from '../types';

type SortOrder = 'asc' | 'desc';


@Pipe({
  name: 'sortGames',
})
export class SortGamesPipe implements PipeTransform {
  transform(
    values: Game[],
    sortKey: GamesSortKeys,
  ): Game[] {
    if (!values) return null 
    if (!sortKey) return values

    if (sortKey === 'league') {
      return values.sort((a, b) => a.league.localeCompare(b.league));
    }

    if (sortKey === 'date') {
      return values.sort((a, b) => {
        const aDate = formatDate(a.date, "yyyy-MM-dd", "en_US");
        const bDate = formatDate(b.date, "yyyy-MM-dd", "en_US");
        if (aDate > bDate) return 1;
        if (aDate < bDate) return -1;
        return 0;
      });
    }

    if (sortKey === 'minimalBettingOdd') {
      return values.sort((a, b) => {
        if (a.minimalBettingOdd > b.minimalBettingOdd) return 1;
        if (a.minimalBettingOdd < b.minimalBettingOdd) return -1;
        return 0;
      });
    }
  }
}
