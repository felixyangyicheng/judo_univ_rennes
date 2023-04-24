#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["judo_univ_rennes.csproj", "."]
RUN dotnet restore "./judo_univ_rennes.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "judo_univ_rennes.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "judo_univ_rennes.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "judo_univ_rennes.dll"]