FROM microsoft/dotnet:1.0.4-core
WORKDIR /app
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "dotnetcore_postgreSQL.dll"]
COPY bin/Debug/netcoreapp1.0/publish /app
