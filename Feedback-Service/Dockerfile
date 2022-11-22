FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# FROM mcr.microsoft.com/dotnet/sdk:6.0-buster-slim AS build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Feedback-Service/Feedback-Service.csproj", "Feedback-Service/"]
RUN dotnet restore "Feedback-Service/Feedback-Service.csproj"
COPY . .
WORKDIR "/src/Feedback-Service"
RUN dotnet build "Feedback-Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Feedback-Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Feedback-Service.dll"]