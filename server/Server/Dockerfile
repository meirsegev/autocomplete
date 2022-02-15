FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base

#####################################################
#####################################################
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /src

#
# Restore
# 

# Copy the main source project files
COPY ./Server.csproj Server/
WORKDIR /src/Server/
RUN dotnet restore -r linux-x64

#
# Build
# 

COPY . /src/Server/
RUN dotnet publish -r linux-x64 --no-restore -c Release -o /app

###############################################################################
###############################################################################

FROM base AS runtime

#
# Add app
#
COPY --from=build /app /app


WORKDIR /app
EXPOSE 5000
ENTRYPOINT ["dotnet", "Server.dll"]