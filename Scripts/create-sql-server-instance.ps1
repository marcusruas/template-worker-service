$CONTAINER_NAME = "worker_template_database"
$SQLSERVER_PASSWORD = "IHeartRainbows44"
$NETWORK_NAME = "worker_network"

Write-Output "Creating SQL Server container named $CONTAINER_NAME"

docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$SQLSERVER_PASSWORD" --network $NETWORK_NAME --name $CONTAINER_NAME -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest

Write-Output "Starting container $CONTAINER_NAME"

docker start $CONTAINER_NAME

Write-Output "Container running."

# CREATE TABLE WORKER_LOGS(
# 	INSTANCENAME VARCHAR(100) NOT NULL,
# 	MESSAGE VARCHAR(1000) NOT NULL,
# 	DATE DATETIME NOT NULL DEFAULT GETDATE()
# )