#! /bin/bash

# Get current version number
version=`dotnet gitversion -output json -showvariable NuGetVersion`
versionPattern='([0-9]+)\.([0-9]+)\.([0-9]+)'

if [[ $version =~ $versionPattern ]] ; then
    major=${BASH_REMATCH[1]}
    minor=${BASH_REMATCH[2]}
    patch=${BASH_REMATCH[3]}

    git tag "v${major}.${minor}.${patch}"
    git push origin
    git push origin --tags

    # Apply tags
    docker tag $MY_DOCKER_REGISTRY/cert-manager-acme-httphook:build $MY_DOCKER_REGISTRY/cert-manager-acme-httphook:latest
    docker tag $MY_DOCKER_REGISTRY/cert-manager-acme-httphook:build $MY_DOCKER_REGISTRY/cert-manager-acme-httphook:$major.$minor.$patch
    docker tag $MY_DOCKER_REGISTRY/cert-manager-acme-httphook:build $MY_DOCKER_REGISTRY/cert-manager-acme-httphook:$major.$minor
    docker tag $MY_DOCKER_REGISTRY/cert-manager-acme-httphook:build $MY_DOCKER_REGISTRY/cert-manager-acme-httphook:$major

    # Push images
    docker push $MY_DOCKER_REGISTRY/cert-manager-acme-httphook:latest
    docker push $MY_DOCKER_REGISTRY/cert-manager-acme-httphook:$major.$minor.$patch
    docker push $MY_DOCKER_REGISTRY/cert-manager-acme-httphook:$major.$minor
    docker push $MY_DOCKER_REGISTRY/cert-manager-acme-httphook:$major
fi