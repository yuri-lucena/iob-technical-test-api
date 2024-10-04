FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
RUN apt update && \
    apt install iputils-ping -y
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Iob.Bank.Host/Iob.Bank.Host.csproj", "Iob.Bank.Host/"]
COPY ["Iob.Bank.Application/Iob.Bank.Application.csproj", "Iob.Bank.Application/"]
COPY ["Iob.Bank.Domain/Iob.Bank.Domain.csproj", "Iob.Bank.Domain/"]
COPY ["Iob.Bank.Infra/Iob.Bank.Infra.csproj", "Iob.Bank.Infra/"]
RUN dotnet restore "./Iob.Bank.Host/Iob.Bank.Host.csproj"
COPY . .
WORKDIR "/src/Iob.Bank.Host"
RUN dotnet build "./Iob.Bank.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Iob.Bank.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Iob.Bank.Host.dll"]