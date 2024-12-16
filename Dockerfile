# Step 1: Use the official .NET runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Step 2: Use the .NET SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file(s)
COPY ["Coltium-Test.csproj", "./"]

# Restore dependencies
RUN dotnet restore "./Coltium-Test.csproj"

# Copy the rest of the application code
COPY . .

# Build the application
RUN dotnet publish "./Coltium-Test.csproj" -c Release -o /app/publish

# Step 3: Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# Start the application
ENTRYPOINT ["dotnet", "Coltium-Test.dll"]
