# Defina o nome da imagem e o nome do container desejados
$IMAGE_NAME = "worker_template_image"
$CONTAINER_NAME = "container_worker_template"

Write-Output "Building the Docker Image"

# Constrói a imagem Docker usando o Dockerfile
docker build -t $IMAGE_NAME ../.

Write-Output "Running the app"

docker run --name "$CONTAINER_NAME" -p 8080:80 $IMAGE_NAME --network $NETWORK_NAME

Write-Output "App is running at port 8080"