#! /bin/bash

# Remove previous build image
docker rmi $MY_DOCKER_REGISTRY/cert-manager-acme-httphook:build

# Ensure version information is up to date
dotnet gitversion \
    -updateassemblyinfo src/AssemblyVersionInfo.cs \
    -ensureassemblyinfo \
    -showvariable NuGetVersion

docker buildx build \
    --platform linux/arm64 \
    --tag $MY_DOCKER_REGISTRY/cert-manager-acme-httphook:build \
    --load \
    src
