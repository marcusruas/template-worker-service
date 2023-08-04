FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

COPY WorkerTemplate.Worker.csproj ./WorkerTemplate.Worker/
RUN dotnet restore

COPY . ./

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

WORKDIR /app

COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "WorkerTemplate.Worker.dll"]