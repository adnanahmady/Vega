FROM node:18.20 AS node
FROM mcr.microsoft.com/dotnet/sdk:7.0

COPY --from=node /usr/local/lib/node_modules /usr/local/lib/node_modules
COPY --from=node /usr/local/include/node /usr/local/include/node
COPY --from=node /usr/local/share/man/man1/node.1 /usr/local/share/man/man1/node.1
COPY --from=node /usr/local/share/doc/node /usr/local/share/doc/node
COPY --from=node /usr/local/bin/node /usr/local/bin/node
COPY --from=node /opt/ /opt/
RUN ln -s /usr/local/lib/node_modules/npm/bin/npm-cli.js /usr/local/bin/npm
RUN ln -s /usr/local/lib/node_modules/npm/bin/npx-cli.js /usr/local/bin/npx
RUN ln -s /opt/yarn-$(ls /opt/ | grep yarn | sed 's/yarn-//')/bin/yarn /usr/local/bin/yarn
RUN ln -s /opt/yarn-$(ls /opt/ | grep yarn | sed 's/yarn-//')/bin/yarnpkg /usr/local/bin/yarnpkg

RUN apt-get update && apt-get install -y sudo vim wget nano

RUN useradd -m -s /bin/bash docker \
    && echo 'docker ALL=(ALL) NOPASSWD:ALL' >> /etc/sudoers

