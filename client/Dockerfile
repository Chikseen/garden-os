FROM node:lts-alpine
WORKDIR /app
COPY package*.json /app/
RUN npm install
COPY . /app
RUN npm run build
EXPOSE 80
CMD [ "node", "stream.js", "VUE_APP_PI_HOST=https://gardenapi.drunc.net/" ]
