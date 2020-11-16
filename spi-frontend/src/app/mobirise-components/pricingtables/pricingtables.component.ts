import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { GameService } from 'src/app/common/services/game.service';
import { Game } from 'src/app/common/types';

@Component({
  selector: 'app-pricingtables',
  templateUrl: './pricingtables.component.html',
  styleUrls: ['./pricingtables.component.scss'],
})
export class PricingtablesComponent implements OnInit {
  gameData$: Observable<Game[]>;

  constructor(private readonly gameService: GameService) {
    this.gameData$ = this.gameService.getGamesThisWeek();
  }

  ngOnInit() {
  }
}
