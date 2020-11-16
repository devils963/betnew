FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

ENV RAPID_API_KEY=apiKey

# copy csproj and restore as distinct layers
COPY *.sln .
COPY ConsoleApp1/*.csproj ./ConsoleApp1/
RUN dotnet restore

# copy everything else and build app
COPY ConsoleApp1/. ./ConsoleApp1/
WORKDIR /app/ConsoleApp1
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/ConsoleApp1/out ./

CMD ["sh", "-c", "dotnet SpiFootballPrediction.dll $RAPID_API_KEY"]


