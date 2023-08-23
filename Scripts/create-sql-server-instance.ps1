# CREATING A DATABASE FOR STUDIES
$CONTAINER_NAME = "worker_template_database"
$SQLSERVER_PASSWORD = "IHeartRainbows44"

Write-Output "Creating SQL Server container named $CONTAINER_NAME"

docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$SQLSERVER_PASSWORD" --name $CONTAINER_NAME -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest

Write-Output "Starting container $CONTAINER_NAME"

docker start $CONTAINER_NAME

Write-Output "Container running."

# CREATE TABLE WORKER_MESSAGES(
#     ID UNIQUEIDENTIFIER,
#     INSTANCE_NAME VARCHAR(100),
#     MESSAGE VARCHAR(500),
#     DATE DATETIME
# )