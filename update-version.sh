#! /bin/bash

dotnet gitversion \
    -updateassemblyinfo src/AssemblyVersionInfo.cs \
    -ensureassemblyinfo \
    -showvariable NuGetVersion
