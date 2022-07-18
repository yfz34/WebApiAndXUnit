FROM mcr.microsoft.com/dotnet/aspnet:6.0 As base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 As build
WORKDIR /src
COPY ["WebApiAndXUnit.Api.csproj", "./"]
RUN dotnet restore "WebApiAndXUnit.Api.csproj"
COPY . .
RUN dotnet publish "WebApiAndXUnit.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT [ "dotnet", "WebApiAndXUnit.Api.csproj" ]
