FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY EmployeeHub.Api/EmployeeHub.Api.csproj EmployeeHub.Api/
COPY EmployeeHub.Application/EmployeeHub.Application.csproj EmployeeHub.Application/
COPY EmployeeHub.Domain/EmployeeHub.Domain.csproj EmployeeHub.Domain/
COPY EmployeeHub.Infrastructure/EmployeeHub.Infrastructure.csproj EmployeeHub.Infrastructure/

RUN dotnet restore EmployeeHub.Api/EmployeeHub.Api.csproj

COPY . .

WORKDIR /src/EmployeeHub.Api

RUN dotnet publish \
    -c Release \
    -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5132
EXPOSE 5132

ENTRYPOINT ["dotnet", "EmployeeHub.Api.dll"]