# Teams Calling Bot - Agente de Llamadas Salientes

Agente de inteligencia artificial que realiza llamadas salientes a través de Microsoft Teams para recopilar información específica con transcripción automática.

## 🎯 Características

- ✅ Llamadas salientes proactivas a usuarios de Teams
- ✅ Recopilación de información específica durante la llamada
- ✅ Transcripción automática de conversaciones
- ✅ Orquestación mediante Power Automate
- ✅ Integración con Microsoft Graph API
- ✅ Almacenamiento de datos en Azure

## 🏗️ Arquitectura

```
Power Automate Flow (Trigger)
    ↓
Azure Function (C# - Bot Logic)
    ↓
Microsoft Graph API
    ↓
Microsoft Teams (Llamada saliente)
    ↓
Azure Cognitive Services (Speech-to-Text)
    ↓
Almacenamiento de transcripción
```

## 📋 Prerrequisitos

- ✅ Azure Subscription activa
- ✅ Microsoft 365 con Teams
- ✅ Aplicación registrada en Azure AD
- ✅ Permisos en Microsoft Graph:
  - `Calls.Initiate`
  - `Calls.InitiateGroupCall`
  - `Calls.JoinGroupCall`

## 🚀 Inicio Rápido

### 1. Clonar el repositorio
```bash
git clone https://github.com/LIGITRAAN/teams-calling-bot.git
cd teams-calling-bot
```

### 2. Configurar Azure AD
Ver: `docs/AZURE_AD_SETUP.md`

### 3. Configurar Azure Function
Ver: `docs/AZURE_FUNCTION_SETUP.md`

### 4. Importar Power Automate Flow
Ver: `docs/POWER_AUTOMATE_SETUP.md`

## 📁 Estructura del Proyecto

```
teams-calling-bot/
├── src/
│   ├── CallingBotFunction.cs          # Función principal
│   ├── Models/
│   │   ├── CallRequest.cs
│   │   ├── CallResponse.cs
│   │   └── TranscriptionData.cs
│   ├── Services/
│   │   ├── GraphApiService.cs         # Integración Graph API
│   │   ├── TranscriptionService.cs    # Speech-to-Text
│   │   └── DataService.cs             # Almacenamiento
│   └── Utils/
│       └── AuthHelper.cs              # Autenticación
├── docs/
│   ├── AZURE_AD_SETUP.md
│   ├── AZURE_FUNCTION_SETUP.md
│   ├── POWER_AUTOMATE_SETUP.md
│   └── API_REFERENCE.md
├── workflows/
│   └── call-outbound.json             # Power Automate Flow
├── .env.example
└── csproj files
```

## 🔐 Variables de Entorno

```env
TENANT_ID=your-tenant-id
CLIENT_ID=your-client-id
CLIENT_SECRET=your-client-secret
SPEECH_KEY=your-speech-services-key
SPEECH_REGION=your-speech-region
STORAGE_CONNECTION_STRING=your-storage-connection-string
```

## 📞 Uso

### Trigger desde Power Automate

```json
{
  "userId": "user@company.com",
  "campaignId": "CAMPAIGN_001",
  "questions": [
    "¿Cuál es tu nombre completo?",
    "¿Cuál es tu email de contacto?",
    "¿Estás interesado en nuestros servicios?"
  ],
  "callbackUrl": "https://your-api.com/webhook"
}
```

### Respuesta del Bot

```json
{
  "callId": "call-12345",
  "status": "completed",
  "duration": 245,
  "transcription": "...",
  "collectedData": {
    "name": "Juan Pérez",
    "email": "juan@example.com",
    "interested": true
  },
  "timestamp": "2026-09-16T10:30:00Z"
}
```

## 🛠️ Desarrollo Local

```bash
# Instalar dependencias
dotnet restore

# Construir
dotnet build

# Ejecutar tests
dotnet test

# Ejecutar localmente con Azure Functions Core Tools
func start
```

## 📚 Documentación Completa

- [Configuración de Azure AD](docs/AZURE_AD_SETUP.md)
- [Setup de Azure Function](docs/AZURE_FUNCTION_SETUP.md)
- [Setup de Power Automate](docs/POWER_AUTOMATE_SETUP.md)
- [Referencia de API](docs/API_REFERENCE.md)

## 🤝 Contribuir

1. Fork el proyecto
2. Crea una rama (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📄 Licencia

MIT - ver LICENSE.md

## 📞 Soporte

Para reportar bugs o sugerencias, abre un [Issue](https://github.com/LIGITRAAN/teams-calling-bot/issues)

---

**Creado por:** LIGITRAAN  
**Última actualización:** Septiembre 2026