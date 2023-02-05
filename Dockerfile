FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80   
ENV ASPNETCORE_URLS=http://+:8001;http://+:80;
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/4-Presentation/pnb.api.easybox.web/pnb.api.easybox.web.csproj", "src/4-Presentation/pnb.api.easybox.web/"]
COPY ["src/2-Domain/pnb.api.easybox.domain/pnb.api.easybox.domain.csproj", "src/2-Domain/pnb.api.easybox.domain/"]
COPY ["src/5-CrossCutting/pnb.api.easybox.dependencyinjection/pnb.api.easybox.dependencyinjection.csproj", "src/5-CrossCutting/pnb.api.easybox.dependencyinjection/"]
COPY ["src/1-Aplication/pnb.api.easybox.aplication/pnb.api.easybox.aplication.csproj", "src/1-Aplication/pnb.api.easybox.aplication/"]
COPY ["src/3-Infra/pnb.api.easybox.proxy/pnb.api.easybox.proxy.csproj", "src/3-Infra/pnb.api.easybox.proxy/"]
COPY ["src/3-Infra/pnb.api.easybox.repository/pnb.api.easybox.repository.csproj", "src/3-Infra/pnb.api.easybox.repository/"]
RUN dotnet restore "src/4-Presentation/pnb.api.easybox.web/pnb.api.easybox.web.csproj"
COPY . .
WORKDIR "/src/src/4-Presentation/pnb.api.easybox.web"
RUN dotnet build "pnb.api.easybox.web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "pnb.api.easybox.web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "pnb.api.easybox.web.dll"]