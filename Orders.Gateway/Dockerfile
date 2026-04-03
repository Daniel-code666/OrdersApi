FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Orders.Gateway/Orders.Gateway.csproj", "Orders.Gateway/"]

RUN dotnet restore "Orders.Gateway/Orders.Gateway.csproj"

COPY . .

WORKDIR "/src/Orders.Gateway"
RUN dotnet publish "Orders.Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Orders.Gateway.dll"]