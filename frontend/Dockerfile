FROM node:12.13.0-alpine3.10 AS build-env

WORKDIR /app

COPY . /app

RUN npm install
RUN npm run-script build

FROM node:12.13.0-alpine3.10

WORKDIR /app

RUN npm install -g serve@11.2.0

COPY --from=build-env /app/build /app

CMD [ "serve", "-s", "-l", "443", "/app" ]