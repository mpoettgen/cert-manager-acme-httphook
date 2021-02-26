#! /bin/bash

# Create/refresh version information
dotnet gitversion \
    -updateassemblyinfo src/AssemblyVersionInfo.cs \
    -ensureassemblyinfo \
    -showvariable NuGetVersion
