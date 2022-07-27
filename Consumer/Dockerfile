FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["Consumer/Consumer.csproj", "Consumer/"]
RUN dotnet restore "Consumer/Consumer.csproj"
COPY . .
WORKDIR "/src/Consumer"
RUN dotnet build "Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Consumer.dll"]
