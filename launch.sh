#!/bin/bash

dotnet run --project src/Services/Company/Company.API/Company.API.csproj --urls="http://+:3000" &
COMPANY_PID=$!
echo COMPANY_PID

# dotnet run --project src/Services/Product/Product.API/Product.API.csproj --urls="http://+:3100" &