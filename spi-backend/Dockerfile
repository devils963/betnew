FROM node:12.7-alpine AS build

ENV DB_SERVER_NAME=localhost

ENV DB_SERVER_PORT=2717

ENV CREATE_GAME_SECRET=

WORKDIR /usr/graphql-server
COPY package.json /usr/graphql-server/

RUN npm install
COPY ./ /usr/graphql-server

RUN npm run build
COPY ./ /usr/graphql-server

EXPOSE 4000

CMD ["sh", "-c", "node dist/index.js $DB_SERVER_NAME $DB_SERVER_PORT $CREATE_GAME_SECRET"]