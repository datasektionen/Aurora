FROM docker.io/node:23-alpine3.20 AS build
WORKDIR /app

COPY package.json package-lock.json ./

RUN npm ci

COPY gruntfile.js ./
COPY css/ css/

RUN npm run build

FROM docker.io/busybox:1-musl
WORKDIR /app

RUN echo -e "I:index.css" > /httpd.conf
COPY --from=build /app/css/compiled.css /app/index.css

EXPOSE 3000
CMD ["httpd", "-fp3000", "-c/httpd.conf"]
