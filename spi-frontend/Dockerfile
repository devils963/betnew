FROM node:12.7-alpine AS build

COPY package.json /usr/angular-workdir/
WORKDIR /usr/angular-workdir
RUN npm install

COPY ./ /usr/angular-workdir
RUN npm run build

FROM nginx:1.15.8-alpine

RUN rm -rf /usr/share/nginx/html/*

COPY --from=build  /usr/angular-workdir/dist/mobirise-angular-integration /usr/share/nginx/html

##some change