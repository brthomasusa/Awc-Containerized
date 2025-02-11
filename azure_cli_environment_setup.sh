#!/bin/bash

export RESOURCE_GROUP="awc-microservices"
export LOCATION="westus2"
export ENVIRONMENT="env-awc-containerapps"
export API_NAME="web-gateway"
export GITHUB_USERNAME="brthomasusa"
export FRONTEND_NAME="webui"
export ACR_NAME="awcacr"$GITHUB_USERNAME
export IDENTITY="acr-admin"
export IDENTITY_ID=$(az identity show --name $IDENTITY --resource-group $RESOURCE_GROUP --query id --output tsv)