FROM mono
MAINTAINER Edward Wilde

RUN apt-get update && apt-get install -y git wget

RUN mozroots --import --sync

WORKDIR  /home
RUN wget http://nuget.org/nuget.exe
RUN mv /home/nuget.exe /usr/bin/nuget
