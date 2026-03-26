# Use ASP.NET runtime (since you're pointing to a prebuilt DLL)
FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

# Copy your already-built DLL and dependencies
COPY bin/Release/net10.0/ ./

# Tell ASP.NET to listen on HTTP and HTTPS ports
ENV ASPNETCORE_URLS=http://+:5231;https://+:7280

# Expose the ports
EXPOSE 5231
EXPOSE 7280

# Run your app
ENTRYPOINT ["dotnet", "dumpling_connection2.dll"]