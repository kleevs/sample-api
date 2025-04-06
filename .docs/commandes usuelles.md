# Commande utiles

## Cr�ation d'une migration
`dotnet ef migrations add <nom de la migrations> --project src/MyProject.Database/MyProject.Database.csproj --startup-project src/MyProject.Api/MyProject.Api.csproj`

## Mise � jour de la base 
`dotnet ef database update --startup-project ./src/MyProject.Api/MyProject.Api.csproj --project src/MyProject.Database/MyProject.Database.csproj`

## Création du script sql
`dotnet ef migrations script --output carduser.sql --idempotent --context MyAppContext --startup-project ./src/MyProject.Api/MyProject.Api.csproj --project src/MyProject.Database/MyProject.Database.csproj`

## BlobStorage setup
https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=docker-hub

## Sql server setup
https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver16&pivots=cs1-bash

## Docker sql server
`docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=sqllocalpassWord1" -p 1433:1433 --name sql1 --hostname sql1 -d mcr.microsoft.com/mssql/server:2022-latest`

## Ajout d'une autorisation dans la base Card
`INSERT INTO "DOSI"."CARD_RolesPolicies" ("RoleId", "Policy") VALUES (1, '[NOM DE LA POLICY]')`
