FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["Publisher/Publisher.csproj", "Publisher/"]
RUN dotnet restore "Publisher/Publisher.csproj"
COPY . .
WORKDIR "/src/Publisher"
RUN dotnet build "Publisher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Publisher.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Publisher.dll"]
