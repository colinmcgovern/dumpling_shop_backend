# Use ASP.NET runtime (since you're pointing to a prebuilt DLL)
FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

# Copy your already-built DLL and dependencies
COPY bin/Release/net10.0/ ./

# Tell ASP.NET to listen on port 5231
ENV ASPNETCORE_URLS=http://+:5231

# Expose the port
EXPOSE 5231

# Run your app
ENTRYPOINT ["dotnet", "dumpling_connection2.dll"]