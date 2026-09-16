# Referencia de API

## Base URL

```
https://teams-calling-bot-func.azurewebsites.net/api
```

## Endpoints

### 1. Iniciar Llamada Saliente

**POST** `/call/outbound`

Inicia una llamada saliente a un usuario de Teams.

#### Request

```json
{
  "userId": "user@company.com",
  "campaignId": "CAMPAIGN_001",
  "welcomeMessage": "Hola, esta es una llamada automatizada",
  "questions": [
    "¿Cuál es tu nombre completo?",
    "¿Cuál es tu email de contacto?"
  ],
  "callbackUrl": "https://your-webhook-url.com/webhook",
  "contextData": {
    "key1": "value1"
  },
  "language": "es-ES",
  "maxDurationSeconds": 600
}
```

#### Response (200 OK)

```json
{
  "callId": "call-abc123xyz",
  "status": "initiated",
  "duration": 0,
  "startTime": "2026-09-16T10:30:00Z",
  "transcription": null
}
```

#### Response (500 Error)

```json
{
  "callId": null,
  "status": "failed",
  "errorMessage": "Descripción del error"
}
```

---

### 2. Obtener Estado de Llamada

**GET** `/call/{callId}/status`

Obtiene el estado actual de una llamada.

#### Parameters

| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `callId` | string | ID de la llamada |

#### Response (200 OK)

```json
{
  "callId": "call-abc123xyz",
  "status": "connected",
  "duration": 45
}
```

---

### 3. Finalizar Llamada

**POST** `/call/{callId}/end`

Finaliza una llamada en progreso.

#### Parameters

| Parámetro | Tipo | Descripción |
|-----------|------|-------------|
| `callId` | string | ID de la llamada |

#### Response (200 OK)

```json
{
  "message": "Llamada finalizada exitosamente"
}
```

---

### 4. Webhook de Callback

**POST** `/call/webhook`

Recibe eventos de la llamada.

#### Body

```json
{
  "callId": "call-abc123xyz",
  "event": "call_started|call_ended|dtmf_received",
  "data": {}
}
```

#### Response (200 OK)

```json
{
  "message": "Webhook procesado"
}
```

---

## Estados de Llamada

| Estado | Descripción |
|--------|-------------|
| `initiated` | Llamada iniciada |
| `ringing` | Teléfono sonando |
| `connected` | Llamada conectada |
| `completed` | Llamada finalizada |
| `failed` | Error en la llamada |

---

## Códigos de Error

| Código | Mensaje | Solución |
|--------|---------|----------|
| 400 | UserId es requerido | Verifica que userId esté presente |
| 400 | Al menos una pregunta es requerida | Agrega al menos una pregunta |
| 401 | Unauthorized | Verifica las credenciales de Azure AD |
| 403 | Forbidden | Verifica los permisos en Microsoft Graph |
| 500 | Error interno | Verifica los logs de la Function App |

---

## Ejemplos de Uso

### cURL

```bash
curl -X POST https://teams-calling-bot-func.azurewebsites.net/api/call/outbound \
  -H "Content-Type: application/json" \
  -d '{
    "userId": "user@company.com",
    "campaignId": "CAMPAIGN_001",
    "questions": ["¿Cómo estás?"],
    "callbackUrl": "https://your-webhook.com"
  }'
```

### Python

```python
import requests

url = "https://teams-calling-bot-func.azurewebsites.net/api/call/outbound"
payload = {
    "userId": "user@company.com",
    "campaignId": "CAMPAIGN_001",
    "questions": ["¿Cómo estás?"],
    "callbackUrl": "https://your-webhook.com"
}

response = requests.post(url, json=payload)
print(response.json())
```

### PowerShell

```powershell
$uri = "https://teams-calling-bot-func.azurewebsites.net/api/call/outbound"
$body = @{
    userId = "user@company.com"
    campaignId = "CAMPAIGN_001"
    questions = @("¿Cómo estás?")
    callbackUrl = "https://your-webhook.com"
} | ConvertTo-Json

Invoke-WebRequest -Uri $uri -Method POST -Body $body -ContentType "application/json"
```
