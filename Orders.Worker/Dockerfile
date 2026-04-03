FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Orders.Worker/Orders.Worker.csproj", "Orders.Worker/"]

RUN dotnet restore "Orders.Worker/Orders.Worker.csproj"

COPY . .

WORKDIR "/src/Orders.Worker"
RUN dotnet publish "Orders.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Orders.Worker.dll"]