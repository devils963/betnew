import { Game, IGame } from './models/game';
import mongoose from 'mongoose';

export const resolvers = {
  Query: {
    games: (): mongoose.DocumentQuery<IGame[], IGame> => Game.find(),
    gamesThisWeek: (): mongoose.DocumentQuery<IGame[], IGame> => {
      const date = new Date();
      const futureDate = new Date();
      futureDate.setDate(date.getDate() + 7);
      return Game.find({ date: { $lt: futureDate, $gt: date } });
    },
    game: (
      _: unknown,
      args: { id: string }
    ): mongoose.DocumentQuery<IGame | null, IGame> => Game.findById(args.id)
  },
  Mutation: {
    createGame: async (
      _: unknown,
      gameToCreate: IGame
    ): mongoose.DocumentQuery<IGame | null, IGame> => {
      const foundGame = await Game.findOne({
        date: gameToCreate.date,
        homeTeam: gameToCreate.homeTeam,
        awayTeam: gameToCreate.awayTeam
      });

      if (!foundGame) {
        const game = new Game({
          ...gameToCreate,
          id: new mongoose.Types.ObjectId()
        });
        return await Game.create(game);
      }
      return foundGame;
    }
  }
};
