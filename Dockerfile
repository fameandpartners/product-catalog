FROM microsoft/dotnet:2.1-sdk AS build-env
ARG ConnectionStrings__FameConnection=stub-value-from-dockerfile

WORKDIR /app
# copy csproj and restore as distinct layers
COPY *.sln ./
COPY Fame.Service.UnitTest/Fame.Service.UnitTest.csproj ./Fame.Service.UnitTest/
COPY Fame.Background/Fame.Background.csproj ./Fame.Background/
COPY Fame.Data/Fame.Data.csproj ./Fame.Data/
COPY Fame.ImageGeneratorTest/Fame.ImageGeneratorTest.csproj ./Fame.ImageGeneratorTest/
COPY Fame.Web/Fame.Web.csproj ./Fame.Web/
COPY Fame.ImageGenerator/Fame.ImageGenerator.csproj ./Fame.ImageGenerator/
COPY Fame.Service/Fame.Service.csproj ./Fame.Service/
COPY Fame.Common/Fame.Common.csproj ./Fame.Common/
COPY Fame.Search/Fame.Search.csproj ./Fame.Search/
COPY Fame.Service.Integration/Fame.Service.Integration.csproj ./Fame.Service.Integration/

RUN dotnet restore

# copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# run yarn
FROM node:8.11-alpine AS node-builder
WORKDIR /app/Fame.Web
COPY ./Fame.Web/package.json /app/Fame.Web/
COPY ./Fame.Web/package-lock.json /app/Fame.Web/
RUN npm install
COPY ./Fame.Web /app/Fame.Web/
RUN npm run build

# build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime
ARG CIRCLE_BUILD_NUM
ENV CIRCLE_BUILD_NUM=${CIRCLE_BUILD_NUM}
ARG CIRCLE_BRANCH
ENV CIRCLE_BRANCH=${CIRCLE_BRANCH}
ARG CIRCLE_SHA1
ENV CIRCLE_SHA1=${CIRCLE_SHA1}
WORKDIR /app

ENV CORECLR_ENABLE_PROFILING=1 \
CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
CORECLR_NEWRELIC_HOME=/usr/local/newrelic-netcore20-agent \
CORECLR_PROFILER_PATH=/usr/local/newrelic-netcore20-agent/libNewRelicProfiler.so
RUN curl https://download.newrelic.com/dot_net_agent/previous_releases/8.6.45.0/newrelic-netcore20-agent_8.6.45.0_amd64.deb -O
RUN dpkg -i ./newrelic-netcore2*.deb

EXPOSE 80
COPY --from=build-env /app/Fame.Web/out .
COPY --from=node-builder /app/Fame.Web/wwwroot/dist ./wwwroot/dist

ENTRYPOINT ["dotnet", "Fame.Web.dll"]
