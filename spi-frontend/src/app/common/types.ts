export interface Game {
    league: string;
    homeTeam: string;
    awayTeam: string;
    homeTeamIsWinningSide: boolean;
    minimalBettingOdd: number;
    date: Date;
}
