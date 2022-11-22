# FROM mcr.microsoft.com/dotnet/aspnet:6.0-buster-slim AS base
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

WORKDIR /app

EXPOSE 7034

# FROM mcr.microsoft.com/dotnet/sdk:6.0-buster-slim AS build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY "Feedback-Service.csproj" .
RUN dotnet restore "Feedback-Service.csproj"
# Copy everything into the docker directory and build
COPY . .
RUN dotnet build "Feedback-Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Feedback-Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Feedback-Service.dll"]
