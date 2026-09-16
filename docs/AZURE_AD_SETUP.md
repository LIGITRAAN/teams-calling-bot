# Configuración de Azure AD

## Paso 1: Registrar la Aplicación

1. Ve a [Azure Portal](https://portal.azure.com)
2. Navega a **Azure Active Directory** → **App registrations** → **New registration**
3. Ingresa un nombre: `TeamsCallingBot`
4. Selecciona **Accounts in any organizational directory (Any Azure AD directory - Multitenant)**
5. Click en **Register**

## Paso 2: Obtener Credenciales

1. Copia el **Application (client) ID** - lo usarás como `CLIENT_ID`
2. Copia el **Directory (tenant) ID** - lo usarás como `TENANT_ID`
3. Ve a **Certificates & secrets**
4. Click en **New client secret**
5. Ingresa una descripción y selecciona expiration
6. Copia el valor - lo usarás como `CLIENT_SECRET`

## Paso 3: Configurar Permisos de API

1. Ve a **API permissions**
2. Click en **Add a permission**
3. Selecciona **Microsoft Graph**
4. Selecciona **Application permissions**
5. Busca y agrega los siguientes permisos:
   - `Calls.Initiate`
   - `Calls.InitiateGroupCall`
   - `Calls.JoinGroupCall`
   - `Calls.AccessMedia`

6. Click en **Grant admin consent for [Organization]**

## Paso 4: Configurar Redirect URI

1. Ve a **Authentication**
2. Click en **Add a platform**
3. Selecciona **Web**
4. Ingresa: `https://localhost:7071/api/callback`
5. Marca `Access tokens` y `ID tokens`
6. Click en **Configure**

## Paso 5: Verificar Permisos

Verifica que los siguientes permisos estén concedidos:

```
✅ Calls.Initiate
✅ Calls.InitiateGroupCall
✅ Calls.JoinGroupCall
✅ Calls.AccessMedia
```

## Próximo Paso

Continúa con [Azure Function Setup](AZURE_FUNCTION_SETUP.md)
