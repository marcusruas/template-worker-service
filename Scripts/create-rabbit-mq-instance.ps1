$CONTAINER_NAME = "messaging_queue"
$RABBITMQ_USER = "worker_service"
$RABBITMQ_PASSWORD = "IHeartRainbows44"
$RABBITMQ_PORT = 15672

Write-Output "Creating RabbitMQ container named $CONTAINER_NAME"

# Creating RabbitMQ Queue for worker.
docker run -d --name $CONTAINER_NAME -p 5672:5672 -p $RABBITMQ_PORT:$RABBITMQ_PORT -e RABBITMQ_DEFAULT_USER=$RABBITMQ_USER -e RABBITMQ_DEFAULT_PASS=$RABBITMQ_PASSWORD --network $NETWORK_NAME rabbitmq:management

Write-Output "Starting container $CONTAINER_NAME"

docker start $CONTAINER_NAME

Write-Output "Container running. The management panel can be accessed at http://localhost:$RABBITMQ_PORT"