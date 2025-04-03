FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY FormulaOne.Entities/FormulaOne.Entities.csproj FormulaOne.Entities/
COPY FormulaOne.DataService/FormulaOne.DataService.csproj FormulaOne.DataService/
COPY FormulaOne.Services/FormulaOne.Services.csproj FormulaOne.Services/
COPY FormulaOne.Api/FormulaOne.Api.csproj FormulaOne.Api/
RUN dotnet restore FormulaOne.Api/FormulaOne.Api.csproj
COPY . .
WORKDIR /src/FormulaOne.Api
RUN dotnet build FormulaOne.Api.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish FormulaOne.Api.csproj -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "FormulaOne.Api.dll" ]