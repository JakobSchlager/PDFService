FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
RUN apt-get update && apt-get install -y libgdiplus

WORKDIR /app

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["PDFService/PDFService/PDFService.csproj", "PDFService/PDFService/"]
RUN dotnet restore "PDFService/PDFService/PDFService.csproj"
COPY . .
WORKDIR "/src/PDFService/PDFService"
RUN dotnet build "PDFService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PDFService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PDFService.dll"]
