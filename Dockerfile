FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WorkerTemplate.Worker/WorkerTemplate.Worker.csproj", "WorkerTemplate.Worker/"]
COPY ["WorkerTemplate.SharedKernel/WorkerTemplate.SharedKernel.csproj", "WorkerTemplate.SharedKernel/"]
COPY ["WorkerTemplate.Infrastructure/WorkerTemplate.Infrastructure.csproj", "WorkerTemplate.Infrastructure/"]
COPY ["WorkerTemplate.Domain/WorkerTemplate.Domain.csproj", "WorkerTemplate.Domain/"]
RUN dotnet restore "WorkerTemplate.Worker/WorkerTemplate.Worker.csproj"
COPY . .
WORKDIR "/src/WorkerTemplate.Worker"
RUN dotnet build "WorkerTemplate.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorkerTemplate.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkerTemplate.Worker.dll"]