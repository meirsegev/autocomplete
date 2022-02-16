# NodeJS Engine
FROM node:12-alpine AS builder

# install NPM packages, (copying npm config and install them)
WORKDIR "/src/app"

# npm install
COPY ["client/package*.json", "/src/app/"]
RUN npm install

# build app
COPY ["client/", "/src/app/"]
RUN npm run build:prod

# copy the front end app and serve the app with nginx
FROM nginx:1.19-alpine AS runtime

# copy the nginx.confg as template because it has env variables in it
COPY ["client/nginx/nginx.conf", "/etc/nginx/templates/default.conf.template"]
COPY --from=builder /src/app/dist/client /app
RUN ls -ll /app

EXPOSE 80

# run the docker-entrypoint.sh, will replace the envs with values and output  
# the default.conf file under /etc/nginx/conf.d/
ENTRYPOINT ["/docker-entrypoint.sh"]
CMD ["nginx", "-g", "daemon off;"]
