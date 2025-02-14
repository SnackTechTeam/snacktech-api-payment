FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /src

COPY src/common/common.csproj common/
COPY src/driver.amazon.sqs/driver.amazon.sqs.csproj driver.amazon.sqs/
COPY src/driver.database.mongo/driver.database.mongo.csproj driver.database.mongo/
COPY src/driver.mercado-pago/driver.mercado-pago.csproj driver.mercado-pago/
COPY src/core/core.csproj core/
COPY src/api/api.csproj api/

RUN dotnet restore api/api.csproj

COPY src/common/ common/
COPY src/driver.amazon.sqs/ driver.amazon.sqs/
COPY src/driver.database.mongo/ driver.database.mongo/
COPY src/driver.mercado-pago/ driver.mercado-pago/
COPY src/core/ core/
COPY src/api/ api/

RUN dotnet build api/api.csproj -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /src

COPY --from=build-env /app/build .

EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "api.dll"]