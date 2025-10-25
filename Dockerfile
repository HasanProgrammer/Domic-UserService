FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/Presentation/Domic.WebAPI/Domic.WebAPI.csproj", "src/Presentation/Domic.WebAPI/"]
COPY ["src/Infrastructure/Domic.Infrastructure/Domic.Infrastructure.csproj", "src/Infrastructure/Domic.Infrastructure/"]
COPY ["src/Infrastructure/Domic.Persistence/Domic.Persistence.csproj", "src/Infrastructure/Domic.Persistence/"]
COPY ["src/Core/Domic.UseCase/Domic.UseCase.csproj", "src/Core/Domic.UseCase/"]
COPY ["src/Core/Domic.Domain/Domic.Domain.csproj", "src/Core/Domic.Domain/"]
COPY ["src/Core/Domic.Common/Domic.Common.csproj", "src/Core/Domic.Common/"]
RUN dotnet restore "src/Presentation/Domic.WebAPI/Domic.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Presentation/Domic.WebAPI"
RUN dotnet build "Domic.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Domic.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Domic.WebAPI.dll"]