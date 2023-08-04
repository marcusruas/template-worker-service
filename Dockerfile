FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

COPY ./WorkerTemplate.sln ./
COPY ./WorkerTemplate.Domain/WorkerTemplate.Domain.csproj WorkerTemplate.Domain/
COPY ./WorkerTemplate.Infrastructure/WorkerTemplate.Infrastructure.csproj WorkerTemplate.Infrastructure/
COPY ./WorkerTemplate.SharedKernel/WorkerTemplate.SharedKernel.csproj WorkerTemplate.SharedKernel/
COPY ./WorkerTemplate.Worker/WorkerTemplate.Worker.csproj WorkerTemplate.Worker/
RUN dotnet restore

COPY WorkerTemplate.Worker/ ./WorkerTemplate.Worker/
WORKDIR /app/WorkerTemplate.Worker

RUN dotnet publish -c Release -o out WorkerTemplate.Worker.csproj

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /src/WorkerTemplate.Worker/out ./

ENTRYPOINT ["dotnet", "WorkerTemplate.Worker.dll"]