#!/bin/bash

. ~/.profile

dotnet run --project src/Services/Company/Company.API/Company.API.csproj &

dotnet run --project src/Services/Product/Product.API/Product.API.csproj &