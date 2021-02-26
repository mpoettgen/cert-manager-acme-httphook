#! /bin/bash

# Remove previous build image
docker rmi cert-manager-acme-httphook:build

# Ensure version information is up to date
dotnet gitversion \
    -updateassemblyinfo src/AssemblyVersionInfo.cs \
    -ensureassemblyinfo \
    -showvariable NuGetVersion

# Build locally: docker buildx build -f src/cert-manager-acme-httphook/Dockerfile -t cert-manager-acme-httphook:build --platform arm64 src
# Build in buildkitd remotely
buildctl --addr tcp://$MY_DOCKER_BUILD_HOST:$MY_DOCKER_BUILD_PORT \
    --tlscert $MY_CLIENT_CERT \
    --tlscacert $MY_CLIENT_CERT_CA \
    --tlskey $MY_CLIENT_CERT_KEY \
    build \
    --frontend dockerfile.v0 \
    --local context=src \
    --local dockerfile=src/cert-manager-acme-httphook \
    --output type=docker,name=cert-manager-acme-httphook:build \
    | docker load

# Doesn't work: --output type=image,name=$MY_DOCKER_REGISTRY/cert-manager-acme-httphook,push=true
