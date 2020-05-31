FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app
COPY *.sln ./
COPY IT.BFF.Domain.Contracts/*.csproj IT.BFF.Domain.Contracts/
COPY IT.BFF.Domain.Core/*.csproj IT.BFF.Domain.Core/
COPY IT.BFF.Infra.TelegraphConnector/*.csproj IT.BFF.Infra.TelegraphConnector/
COPY IT.BFF.WebApi/*.csproj IT.BFF.WebApi/
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Production
COPY --from=build-env /app/out .
RUN mkdir -p logs
EXPOSE 80 443
ENTRYPOINT ["dotnet", "IT.BFF.WebApi.dll"]
