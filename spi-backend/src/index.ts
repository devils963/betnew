import { ApolloServer } from 'apollo-server-express';
import cors from 'cors';
import express from 'express';
import mongoose from 'mongoose';
import { resolvers } from './resolvers';
import { typeDefs } from './typeDefs';

const DB_HOST = process.argv[2];
const DB_PORT = process.argv[3];

const startServer = async () => {
  const app = express();

  app.use(cors());
  const server = new ApolloServer({
    typeDefs,
    resolvers
  });

  server.applyMiddleware({ app });

  await mongoose.connect(`mongodb://${DB_HOST}:${DB_PORT}/prod`, {
    useNewUrlParser: true
  });

  app.listen({ port: 4000 }, () => {
    console.log(`Successfully connected to DB on ${DB_HOST}:${DB_PORT}`);
  });
};

startServer();
