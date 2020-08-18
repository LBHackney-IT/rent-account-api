FROM mcr.microsoft.com/dotnet/core/sdk:3.1

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./RentAccountApi.sln ./
COPY ./RentAccountApi/RentAccountApi.csproj ./RentAccountApi/
COPY ./RentAccountApi.Tests/RentAccountApi.Tests.csproj ./RentAccountApi.Tests/

RUN dotnet restore ./RentAccountApi/RentAccountApi.csproj
RUN dotnet restore ./RentAccountApi.Tests/RentAccountApi.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c debug -o out RentAccountApi.Tests/RentAccountApi.Tests.csproj

CMD dotnet test
