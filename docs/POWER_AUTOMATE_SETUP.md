# Configuración de Power Automate

## Paso 1: Crear un Cloud Flow

1. Ve a [Power Automate](https://powerautomate.microsoft.com)
2. Click en **+ Create**
3. Selecciona **Cloud flows** → **Automated cloud flow**
4. Dale un nombre: `Initiate Teams Call`
5. Selecciona un trigger (por ejemplo: `When an item is created` si usas SharePoint)
6. Click en **Create**

## Paso 2: Agregar Acción HTTP

1. Click en **+ New step**
2. Busca `HTTP`
3. Selecciona la acción **HTTP**
4. Configura:
   - **Method:** POST
   - **URI:** `https://teams-calling-bot-func.azurewebsites.net/api/call/outbound`
   - **Headers:**
     ```
     Content-Type: application/json
     ```
   - **Body:**
     ```json
     {
       "userId": "user@company.com",
       "campaignId": "CAMPAIGN_001",
       "welcomeMessage": "Hola, esta es una llamada automatizada",
       "questions": [
         "¿Cuál es tu nombre completo?",
         "¿Cuál es tu email de contacto?",
         "¿Estás interesado en nuestros servicios?"
       ],
       "callbackUrl": "https://teams-calling-bot-func.azurewebsites.net/api/call/webhook",
       "contextData": {},
       "language": "es-ES"
     }
     ```

## Paso 3: Procesar Respuesta

1. Click en **+ New step**
2. Busca `Parse JSON`
3. Configura:
   - **Content:** Output de la acción HTTP anterior
   - **Schema:**
     ```json
     {
       "type": "object",
       "properties": {
         "callId": { "type": "string" },
         "status": { "type": "string" },
         "duration": { "type": "integer" }
       }
     }
     ```

## Paso 4: Agregar Notificación (Opcional)

1. Click en **+ New step**
2. Busca `Send an email notification`
3. Configura:
   - **To:** tu email
   - **Subject:** Llamada iniciada
   - **Body:** `Call ID: @{body('Parse_JSON')?['callId']}`

## Paso 5: Publicar el Flow

1. Click en **Save**
2. El flujo ahora está activo

## Ejemplo de Trigger: SharePoint List

Si quieres que el flujo se dispare cuando se agregue un nuevo elemento a una lista de SharePoint:

1. Selecciona trigger: **When an item is created** (SharePoint)
2. Configura Site y List
3. En el Body de HTTP, mapea las propiedades dinámicamente:
   ```json
   {
     "userId": "@{triggerBody()?['email']}",
     "campaignId": "@{triggerBody()?['campaignId']}",
     "questions": @{triggerBody()?['questions']},
     "callbackUrl": "https://teams-calling-bot-func.azurewebsites.net/api/call/webhook"
   }
   ```

## Monitoreo

1. Ve a la ejecución del flow
2. Verifica los logs en cada paso
3. Puedes agregar `Compose` actions para debuggear

## Próximo Paso

Continúa con [API Reference](API_REFERENCE.md)
