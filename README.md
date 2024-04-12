# PluralSight HighLoad

This is a test project to create some high load on both, CPU and memory to have a "heavy hitter"used in a monitoring PoC for sustainability.

## Containerize using .net8

Using the container related project settings, the following command will actually upload a 'dockerized' version to the GitHub CR. [More infos](https://laurentkempe.com/2023/10/30/publish-dotnet-docker-images-using-dotnet-sdk-and-github-actions/)

```bash
publish /HighLoad.Console/HighLoad.Console.csproj --os linux --arch x64 /t:PublishContainer -c Release
```

## Use docker

```bash
# create the image
docker build -t highload-image:latest -f Dockerfile .

# Run a single instance
docker run --name highload-once --rm  highload-image 60 8

# Run interactive
docker run -it --name highload --rm highload-image

# Remove image
docker rmi  highload-image
# optionally
undangle
```
