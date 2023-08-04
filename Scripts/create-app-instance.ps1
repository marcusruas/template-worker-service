# Defina o nome da imagem e o nome do container desejados
$ImageName = "worker_template_image"
$ContainerName = "container_worker_template"

Write-Output "Building the Docker Image"

cd "../"

# Constrói a imagem Docker usando o Dockerfile
docker build -t $ImageName .

Write-Output "Running the app"

docker run --name "$ContainerName" -p 8080:80 $ImageName

Write-Output "App is running at port 8080"