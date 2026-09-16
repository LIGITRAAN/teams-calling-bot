# Configuración de Azure Function

## Prerequisitos

- Azure CLI instalado
- .NET SDK 6.0 o superior
- Azure Functions Core Tools

## Paso 1: Crear Storage Account

```bash
# Set variables
RG_NAME="teams-calling-bot-rg"
STORAGE_NAME="teamsbotStorage$(date +%s)"
LOCATION="eastus"

# Crear grupo de recursos
az group create --name $RG_NAME --location $LOCATION

# Crear storage account
az storage account create \
  --name $STORAGE_NAME \
  --resource-group $RG_NAME \
  --location $LOCATION \
  --sku Standard_LRS

# Obtener connection string
az storage account show-connection-string \
  --name $STORAGE_NAME \
  --resource-group $RG_NAME
```

## Paso 2: Crear Azure Function App

```bash
FUNCTION_NAME="teams-calling-bot-func"

az functionapp create \
  --resource-group $RG_NAME \
  --consumption-plan-location $LOCATION \
  --runtime dotnet \
  --runtime-version 6 \
  --functions-version 4 \
  --name $FUNCTION_NAME \
  --storage-account $STORAGE_NAME
```

## Paso 3: Configurar Variabales de Entorno

```bash
az functionapp config appsettings set \
  --name $FUNCTION_NAME \
  --resource-group $RG_NAME \
  --settings \
    TENANT_ID="your-tenant-id" \
    CLIENT_ID="your-client-id" \
    CLIENT_SECRET="your-client-secret" \
    SPEECH_KEY="your-speech-key" \
    SPEECH_REGION="your-speech-region" \
    STORAGE_CONNECTION_STRING="your-storage-connection-string"
```

## Paso 4: Desplegar el Código

```bash
# Clonar y navegar al repositorio
git clone https://github.com/LIGITRAAN/teams-calling-bot.git
cd teams-calling-bot

# Publicar en Azure
func azure functionapp publish $FUNCTION_NAME --build remote
```

## Paso 5: Verificar Deployment

```bash
# Obtener la URL de la función
az functionapp show \
  --name $FUNCTION_NAME \
  --resource-group $RG_NAME \
  --query defaultHostName
```

La URL será similar a: `https://teams-calling-bot-func.azurewebsites.net`

## Paso 6: Crear Blob Storage Container

```bash
CONTAINER_NAME="transcriptions"

az storage container create \
  --account-name $STORAGE_NAME \
  --name $CONTAINER_NAME \
  --auth-mode login
```

## Próximo Paso

Continúa con [Power Automate Setup](POWER_AUTOMATE_SETUP.md)
