FROM node:10
WORKDIR /aethon-mock-api

COPY . /aethon-mock-api

RUN npm install

EXPOSE 3000

ENTRYPOINT [ "node", "index.js" ]