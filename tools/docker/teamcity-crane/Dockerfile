FROM mono
MAINTAINER Edward Wilde

RUN apt-get update && apt-get install -y git wget && apt-get install -y nano
RUN apt-get install default-jre

RUN mozroots --import --sync

WORKDIR  /home
RUN wget http://nuget.org/nuget.exe
RUN mv /home/nuget.exe /usr/bin/nuget

RUN wget http://download-cf.jetbrains.com/teamcity/TeamCity-9.0.tar.gz
RUN tar xfz TeamCity-9.0.tar.gz

ADD buildAgent.properties /home/TeamCity/buildAgent/conf/buildAgent.properties

CMD ["/home/TeamCity/buildAgent/bin/agent.sh", "start"]
