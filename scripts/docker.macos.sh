error() {
    printf "\033[1;31m${1}\033[0m"
}

trace() {
    printf "\033[1;36m${1}\033[0m"
}

trace 'dotnet...'
requiredver='3'
if [ -z $(command -v dotnet) ]; then
    error '\n.NET Core runtime not found. .NET Core v'$requiredver' or higher is required.\n'
    exit 1
else
    currentver=$(dotnet --version)
    if [ $(printf '%s\n' "$requiredver" "$currentver" | sort -V | head -n1) != $requiredver ]; then
        error '\n.NET Core v'$requiredver' or higher is required ('$currentver' currently installed) \n'
        exit 1
    fi
fi
trace ' found!\n'

trace 'docker...'
if [ -z $(command -v docker) ]; then
    error '\nDocker not found. Please install the Docker daemon and CLI tools.\n'
    exit 1
fi
trace ' found!\n'

trace 'Configuring dev cert\n'
sudo dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p "SupahSecretPassw0rd" -t

trace 'Building and composing...\n'
docker-compose down -v
docker-compose up --build