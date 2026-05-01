FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "Apo.Services.CouponAPI_V2/Apo.Services.CouponAPI_V2.csproj"
RUN dotnet publish "Apo.Services.CouponAPI_V2/Apo.Services.CouponAPI_V2.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "Apo.Services.CouponAPI_V2.dll"]
