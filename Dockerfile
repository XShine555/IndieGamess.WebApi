# Multi-stage build for running this ASP.NET Core (.NET 10) Web API on AWS Lambda as a container image.
# It uses AWS Lambda Web Adapter (as a Lambda extension) to translate Lambda events into HTTP requests.

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY WebApi/WebApi.csproj WebApi/
RUN dotnet restore WebApi/WebApi.csproj

COPY . .
RUN dotnet publish WebApi/WebApi.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /var/task

# Lambda Web Adapter (extension)
# The zip is published by awslabs and contains the "lambda-adapter" binary.
ADD https://github.com/awslabs/aws-lambda-web-adapter/releases/latest/download/lambda-adapter.zip /tmp/lambda-adapter.zip

RUN apt-get update \
    && apt-get install -y --no-install-recommends unzip ca-certificates \
    && rm -rf /var/lib/apt/lists/* \
    && mkdir -p /opt/extensions \
    && unzip /tmp/lambda-adapter.zip -d /opt/extensions \
    && chmod +x /opt/extensions/lambda-adapter \
    && rm -f /tmp/lambda-adapter.zip

COPY --from=build /app/publish ./

ENV ASPNETCORE_URLS=http://+:8080 \
    AWS_LWA_PORT=8080 \
    AWS_LWA_READINESS_CHECK_PATH=/health

EXPOSE 8080

CMD ["dotnet", "WebApi.dll"]
