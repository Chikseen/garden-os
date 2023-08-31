FROM node:lts-alpine
WORKDIR /app
COPY package*.json /app/
RUN npm install
COPY . /app
ENV VUE_APP_PI_HOST=https://gardenapi.drunc.net/
ENV VUE_APP_AUTH_REALM=GardenOS-PROD
ENV VUE_APP_AUTH_CLIENT_ID=client
ENV VUE_APP_AUTH_REDIRECT=https://gardenos.drunc.net/overview
ENV VUE_APP_AUTH_LOGOUT=http://gardenos.drunc.net/
RUN npm run build 
EXPOSE 80
CMD [ "node", "stream.js" ]
