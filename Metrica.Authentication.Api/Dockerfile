FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Metrica.Authentication.Api/Metrica.Authentication.Api.csproj", "Metrica.Authentication.Api/"]
COPY ["Metrica.Authentication.Application/Metrica.Authentication.Application.csproj", "Metrica.Authentication.Application/"]
COPY ["Metrica.Authentication.Domain/Metrica.Authentication.Domain.csproj", "Metrica.Authentication.Domain/"]
COPY ["Metrica.Authentication.Infrastructure/Metrica.Authentication.Infrastructure.csproj", "Metrica.Authentication.Infrastructure/"]

RUN dotnet restore "Metrica.Authentication.Api/Metrica.Authentication.Api.csproj"

COPY . .
WORKDIR "/src/Metrica.Authentication.Api"
RUN dotnet publish "Metrica.Authentication.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Metrica.Authentication.Api.dll"]