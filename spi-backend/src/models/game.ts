import * as mongoose from 'mongoose';
import { Document, Schema } from 'mongoose';

export interface IGame extends Document {
  id: string;
  league: string;
  homeTeam: string;
  awayTeam: string;
  probabilityHomeTeamWin: number;
  probabilityAwayTeamWin: number;
  homeTeamIsWinningSide: boolean;
  minimalBettingOdd: number;
  date: Date;
}

const gameSchema = new Schema({
  id: { type: String, required: true, unique: true },
  league: { type: String, required: true },
  homeTeam: { type: String, required: true },
  awayTeam: { type: String, required: true },
  probabilityHomeTeamWin: { type: Number, required: true },
  probabilityAwayTeamWin: { type: Number, required: true },
  homeTeamIsWinningSide: { type: Boolean, required: true },
  minimalBettingOdd: { type: Number, required: true },
  date: { type: Date, required: true }
});

export const Game = mongoose.model<IGame>('Game', gameSchema);
