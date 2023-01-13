FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:6.0.300 AS build
WORKDIR /src
COPY ["cert-manager-acme-httphook/cert-manager-acme-httphook.csproj", "cert-manager-acme-httphook/"]
RUN dotnet restore "cert-manager-acme-httphook/cert-manager-acme-httphook.csproj"
COPY . .
WORKDIR "/src/cert-manager-acme-httphook"
RUN dotnet publish "cert-manager-acme-httphook.csproj" --no-restore -c Release -o /app

FROM --platform=$TARGETPLATFORM mcr.microsoft.com/dotnet/aspnet:7.0.2 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "cert-manager-acme-httphook.dll"]
