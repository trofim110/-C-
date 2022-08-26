FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["testovoe/testovoe.csproj", "testovoe/"]
RUN dotnet restore "testovoe/testovoe.csproj"
COPY . .
WORKDIR "/src/testovoe"
RUN dotnet build "testovoe.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "testovoe.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "testovoe.dll"]
