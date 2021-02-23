#! /bin/bash

# Get current version number
version=`dotnet gitversion -output json -showvariable NuGetVersion`
versionPattern='([0-9]+)\.([0-9]+)\.([0-9]+)'

if [[ $version =~ $versionPattern ]] ; then
    major=${BASH_REMATCH[1]}
    minor=${BASH_REMATCH[2]}
    patch=${BASH_REMATCH[3]}

    git tag $major.$minor.$patch
    git push origin --tags

    # Apply tags
    docker tag cert-manager-acme-httphook:build registry.home.poettgen.de/cert-manager-acme-httphook:latest
    docker tag cert-manager-acme-httphook:build registry.home.poettgen.de/cert-manager-acme-httphook:$major.$minor.$patch
    docker tag cert-manager-acme-httphook:build registry.home.poettgen.de/cert-manager-acme-httphook:$major.$minor
    docker tag cert-manager-acme-httphook:build registry.home.poettgen.de/cert-manager-acme-httphook:$major

    # Push images
    docker push registry.home.poettgen.de/cert-manager-acme-httphook:latest
    docker push registry.home.poettgen.de/cert-manager-acme-httphook:$major.$minor.$patch
    docker push registry.home.poettgen.de/cert-manager-acme-httphook:$major.$minor
    docker push registry.home.poettgen.de/cert-manager-acme-httphook:$major
fi