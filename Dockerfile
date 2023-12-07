FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Presentation/Karami.WebAPI/Karami.WebAPI.csproj", "src/Presentation/Karami.WebAPI/"]
COPY ["src/Infrastructure/Karami.Infrastructure/Karami.Infrastructure.csproj", "src/Infrastructure/Karami.Infrastructure/"]
COPY ["src/Infrastructure/Karami.Persistence/Karami.Persistence.csproj", "src/Infrastructure/Karami.Persistence/"]
COPY ["src/Core/Karami.UseCase/Karami.UseCase.csproj", "src/Core/Karami.UseCase/"]
COPY ["src/Core/Karami.Domain/Karami.Domain.csproj", "src/Core/Karami.Domain/"]
COPY ["src/Core/Karami.Common/Karami.Common.csproj", "src/Core/Karami.Common/"]
RUN dotnet restore "src/Presentation/Karami.WebAPI/Karami.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Presentation/Karami.WebAPI"
RUN dotnet build "Karami.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Karami.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Karami.WebAPI.dll"]