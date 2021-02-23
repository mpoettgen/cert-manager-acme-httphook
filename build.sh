#! /bin/bash

# Remove previous build image
docker rmi cert-manager-acme-httphook:build

dotnet gitversion \
    -updateassemblyinfo src/AssemblyVersionInfo.cs \
    -ensureassemblyinfo \
    -showvariable NuGetVersion

# Build locally: docker buildx build -f src/cert-manager-acme-httphook/Dockerfile -t cert-manager-acme-httphook:build --platform arm64 src
# Build in buildkitd remotely
buildctl --addr tcp://build.home.poettgen.de:1234 \
    --tlscacert /home/michael/weather/intermediate.pem \
    --tlscert /home/michael/weather/wildcard.home.poettgen.de.pem \
    --tlskey /home/michael/weather/privkey.pem build \
    --frontend dockerfile.v0 \
    --local context=src \
    --local dockerfile=src/cert-manager-acme-httphook \
    --output type=docker,name=cert-manager-acme-httphook:build \
    | docker load

# Doesn't work: --output type=image,name=registry.home.poettgen.de/cert-manager-acme-httphook,registry.insecure=true,push=true
