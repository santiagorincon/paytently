FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ./Paytently/Paytently.csproj ./Paytently/
COPY ./AcquirerSimulator/AcquirerSimulator.csproj ./AcquirerSimulator/

RUN dotnet restore "./Paytently/Paytently.csproj"
RUN dotnet restore "./AcquirerSimulator/AcquirerSimulator.csproj"

COPY . .

RUN rm -f /src/AcquirerSimulator/appsettings.json /src/AcquirerSimulator/appsettings.Development.json

ARG PROJECT_TYPE

RUN if [ "$PROJECT_TYPE" = "api" ]; then \
    dotnet publish "./Paytently/Paytently.csproj" -c Release -o /app/publish; \
    elif [ "$PROJECT_TYPE" = "worker" ]; then \
    dotnet publish "./AcquirerSimulator/AcquirerSimulator.csproj" -c Release -o /app/publish; \
    else \
    echo "Invalid PROJECT_TYPE value" && exit 1; \
    fi

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5000