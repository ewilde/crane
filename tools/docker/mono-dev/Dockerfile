FROM mono
MAINTAINER Edward Wilde

RUN apt-get update && apt-get install -y git wget && apt-get install -y nano

RUN mozroots --import --sync

WORKDIR  /home
RUN wget http://nuget.org/nuget.exe
RUN mv /home/nuget.exe /usr/bin/nuget
RUN git clone https://github.com/ewilde/crane.git
