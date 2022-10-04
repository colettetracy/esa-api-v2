#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["libs/GV.Libraries.NotificationServices.dll", "libs/"]
COPY ["libs/GV.DomainModel.dll", "libs/"]
COPY ["libs/GV.Libraries.NotificationServices.Sendgrid.dll", "libs/"]
COPY ["libs/GV.Libraries.NotificationServices.Smtp.dll", "libs/"]
COPY ["ESA.API/ESA.API.csproj", "ESA.API/"]
COPY ["ESA.Core/ESA.Core.csproj", "ESA.Core/"]
COPY ["ESA.Infrastructure/ESA.Infrastructure.csproj", "ESA.Infrastructure/"]

RUN dotnet restore "ESA.API/ESA.API.csproj"
COPY . .
WORKDIR "/src/ESA.API"
RUN dotnet build "ESA.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ESA.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ESA.API.dll"]