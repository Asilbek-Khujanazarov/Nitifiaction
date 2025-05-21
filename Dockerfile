FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["PatientRecoverySystem.NotificationService/PatientRecoverySystem.NotificationService.csproj", "PatientRecoverySystem.NotificationService/"]
COPY ["PatientRecoverySystem.Shared/PatientRecoverySystem.Shared.csproj", "PatientRecoverySystem.Shared/"]
RUN dotnet restore "PatientRecoverySystem.NotificationService/PatientRecoverySystem.NotificationService.csproj"
COPY . .
WORKDIR "/src/PatientRecoverySystem.NotificationService"
RUN dotnet build "PatientRecoverySystem.NotificationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PatientRecoverySystem.NotificationService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PatientRecoverySystem.NotificationService.dll"]