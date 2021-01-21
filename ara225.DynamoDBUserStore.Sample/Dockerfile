FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["ara225.DynamoDBUserStore.Sample.csproj", "./"]
RUN dotnet restore "ara225.DynamoDBUserStore.Sample.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ara225.DynamoDBUserStore.Sample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ara225.DynamoDBUserStore.Sample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ara225.DynamoDBUserStore.Sample.dll"]
