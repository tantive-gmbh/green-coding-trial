# PluralSight HighLoad

This is a test project to create some high load on both, CPU and memory to have a "heavy hitter"used in a monitoring PoC for sustainability.

## Use docker

### Create the image

```bash
docker build -t highload-image:latest -f Dockerfile .
```

### Run a single instance

```bash
docker run --name highload-once --rm  highload-image 60 8
```

### Run interactive

```bash
docker run -it --name highload --rm highload-image
```

### Remove image

```bash
docker rmi  highload-image
undangle
```
