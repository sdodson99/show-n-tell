#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

WORKDIR /app
EXPOSE 80
EXPOSE 443

# ENV ASPNETCORE_URLS=http://+:80;https://+:443
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ShowNTell.API/ShowNTell.API.csproj", "ShowNTell.API/"]
COPY ["ShowNTell.AzureStorage/ShowNTell.AzureStorage.csproj", "ShowNTell.AzureStorage/"]
COPY ["ShowNTell.Domain/ShowNTell.Domain.csproj", "ShowNTell.Domain/"]
COPY ["ShowNTell.EntityFramework/ShowNTell.EntityFramework.csproj", "ShowNTell.EntityFramework/"]
RUN dotnet restore "ShowNTell.API/ShowNTell.API.csproj"
COPY . .
WORKDIR "/src/ShowNTell.API"
RUN dotnet build "ShowNTell.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShowNTell.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShowNTell.API.dll"]