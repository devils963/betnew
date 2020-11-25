import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { map, take, tap } from 'rxjs/operators';
import { GameService } from 'src/app/common/services/game.service';
import { Game, GamesSortKeys } from 'src/app/common/types';

@Component({
  selector: 'app-pricingtables',
  templateUrl: './pricingtables.component.html',
  styleUrls: ['./pricingtables.component.scss'],
})
export class PricingtablesComponent implements OnInit {
  sortingCriteria?: GamesSortKeys;
  gameData$: Observable<Game[]>;

  constructor(private readonly gameService: GameService) {
    this.gameData$ = this.gameService.getGamesThisWeek();
  }

  ngOnInit() {
  }
}
