FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env

# Install Node and Yarn
RUN curl -sL https://deb.nodesource.com/setup_10.x | bash -
RUN apt-get install -y nodejs
RUN npm install -g yarn

# Build app
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

# Use dotnet and openssl to create self signed SSL cert
RUN dotnet dev-certs https -ep softbot-cloud-platform.pfx -p "SupahSecretPassw0rd"
RUN openssl pkcs12 -in softbot-cloud-platform.pfx -nocerts -out softbot-cloud-platform.pem -password pass:SupahSecretPassw0rd -nodes
RUN openssl pkcs12 -in softbot-cloud-platform.pfx -nokeys -out softbot-cloud-platform.crt -password pass:SupahSecretPassw0rd -nodes

# Runtime image with published code
FROM mcr.microsoft.com/dotnet/core/sdk:3.0
WORKDIR /app
COPY --from=build-env /app/out .

# Extract self signed SSL cert and trust it
# COPY --from=build-env /app/softbot-cloud-platform.pfx /https/
COPY --from=build-env /app/softbot-cloud-platform.crt /usr/local/share/ca-certificates/softbot-cloud-platform.crt
RUN update-ca-certificates

ENTRYPOINT ["dotnet", "softbot-cloud-platform.dll"]