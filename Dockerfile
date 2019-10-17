FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env
RUN curl -sL https://deb.nodesource.com/setup_10.x | bash -
RUN apt-get install -y nodejs
RUN npm install -g yarn
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime
FROM mcr.microsoft.com/dotnet/core/sdk:3.0
WORKDIR /app
COPY --from=build-env /app/out .

# the server.urls parameter allows dotnet to listen to all incoming requests, not just localhost
ENTRYPOINT ["dotnet", "softbot-cloud-platform.dll", "--server.urls=http://0.0.0.0:5000"]