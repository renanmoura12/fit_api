#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["api_fit.csproj", "./"]
RUN dotnet restore "api_fit.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "api_fit.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "api_fit.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "api_fit.dll"]

CMD ASPNETCORE_URLS="http://*:$PORT" dotnet api_fit.dll