#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HouseRentingSystem/HouseRentingSystem.csproj", "HouseRentingSystem/"]
COPY ["HouseRentingSystem.Core/HouseRentingSystem.Core.csproj", "HouseRentingSystem.Core/"]
COPY ["HouseRentingSystem.Infrastructure/HouseRentingSystem.Infrastructure.csproj", "HouseRentingSystem.Infrastructure/"]
RUN dotnet restore "HouseRentingSystem/HouseRentingSystem.csproj"
COPY . .
WORKDIR "/src/HouseRentingSystem"
RUN dotnet build "HouseRentingSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HouseRentingSystem.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HouseRentingSystem.dll"]