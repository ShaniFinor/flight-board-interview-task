# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.sln .
COPY FlightBoard.Domain/ FlightBoard.Domain/
COPY FlightBoard.Application/ FlightBoard.Application/
COPY FlightBoard.Infrastructure/ FlightBoard.Infrastructure/
COPY FlightBoard.Api/ FlightBoard.Api/

WORKDIR /app/FlightBoard.Api
RUN dotnet publish -c Release -o out

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:80

COPY --from=build /app/FlightBoard.Api/out ./
ENTRYPOINT ["dotnet", "FlightBoard.Api.dll"]