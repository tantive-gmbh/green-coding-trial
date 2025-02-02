# PluralSight HighLoad

This is a test project to create some high load on both, CPU and memory to have a "heavy hitter"used in a monitoring PoC for sustainability.

## Containerize using .net8

Using the container related project settings, the following command will actually upload a 'dockerized' version to the GitHub CR. [More infos](https://laurentkempe.com/2023/10/30/publish-dotnet-docker-images-using-dotnet-sdk-and-github-actions/)

```bash
# Console
publish /HighLoad.Console/HighLoad.Console.csproj --os linux --arch x64 /t:PublishContainer -c Release

# API
publish /HighLoad.Api/HighLoad.Api.csproj --os linux --arch x64 /t:PublishContainer -c Release
```

## Use docker

### Console

```bash
# create the image
docker build -t highload-image:latest -f Dockerfile-console .

# just create a container
docker create --name highload-console highload-image:latest

# Run a single instance
docker run --name highload-once --rm  highload-image 60 8

# Run interactive
docker run -it --name highload --rm highload-image

# Remove image
docker rmi  highload-image
```

### API

```bash
# create the image
docker build -t highload:api -f Dockerfile-green .

# just create a container
docker create --name highload-api highload:api

# Run interactive
docker run -it --name highload-api --rm highload:api

# Remove image
docker rmi  highload-api
```

### API with compose

```bash
# create the image
docker compose -f compose-api.yml create

# start it
docker start highload-api

# stop it
docker stop highload-api

docker rm highload-api && docker rmi highload:api
```

## Use the green-coding-io website

Go to the [Green Coding Request](https://metrics.green-coding.io/request.html) page.

Enter the following data (use your own name and email address for notification).

![image](https://github.com/user-attachments/assets/3072a081-92a0-420f-9976-40f15f9a32ea)

Click "Submit Software" and wait for the outcome to be listed at the [Repository](https://metrics.green-coding.io/repositories.html) page.
