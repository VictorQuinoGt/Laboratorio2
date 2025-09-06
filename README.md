# Laboratorio  Gestion de Órdenes (Backend + Frontend)

Sistema para gestionar **Clientes (Person)**, **Items** y **Órdenes** con sus **detalles**.

Incluye:
- **Backend** (ASP.NET Core Web API + EF Core) con entidades: `Person`, `Item`, `Order`, `OrderDetail` y endpoints REST.
- **Frontend** (Angular 17 + Material, standalone) con CRUD de **Clientes** y **Items**, y pantalla **“Realizar Orden”**.

---

## 🧱 Arquitectura (referencial)
```
/backend
  HelloApi/                 # Web API (Controllers, DbContext, Migrations)
  HelloApi.Models/          # Entidades y DTOs
/frontend
  order-frontend/           # Angular 17 + Material
```

---

## ✅ Requisitos
### Backend
- .NET SDK 8.0 (o 7.0 si tu proyecto está en 7)
- SQL Server 

### Frontend
- Node.js 18+
- Angular CLI
---

## ⚙️ Backend — Configuración y ejecución

1) **Cadena de conexión** en `appsettings.json`

**SQL Server**
```json
"ConnectionStrings": {
  "Default": "Server=localhost;Database=NombreBD;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

2) **DbContext** (`Program.cs`)
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"))); // o UseSqlite
```

3) **CORS (dev)**
```csharp
builder.Services.AddCors(opt => {
    opt.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});
var app = builder.Build();
app.UseCors("AllowAll");
```

4) **Migraciones
dotnet ef migrations add InitialCreate
dotnet ef database update
```

5) **Ejecutar**
```bash
dotnet run
```
- Observa puertos (ej. `http://localhost:5131` ).
- Swagger: `https://localhost:7131/swagger/index.html`

### 🌐 Endpoints esperados
- `GET /api/Person`, `GET /api/Person/{id}`, `POST /api/Person`, `PUT /api/Person/{id}`, `DELETE /api/Person/{id}`
- `GET /api/Items`, `GET /api/Items/{id}`, `POST /api/Items`, `PUT /api/Items/{id}`, `DELETE /api/Items/{id}`
- `POST /api/Orders` (crear orden)


### 🔁 Reglas sugeridas para crear orden
- Validar existencia de `Person` e `Items`.
- Tomar `Price` del `Item` salvo override en el detalle.
- Calcular totales de línea y `GrandTotal`.
- Devolver `OrderReadDto` con `lines` y `grandTotal`.

---

## 💻 Frontend — Configuración y ejecución

1) **Instalar dependencias**
```bash
npm install
```

2) **Conectar al backend**

**Opción A — URL directa**
```ts
// src/app/app.config.ts
{ provide: API_BASE, useValue: 'http://localhost:5131' } // o https://localhost:7131
```
```bash
npx ng serve -o
```

**Opción B — Proxy (recomendado en dev)**
`proxy.conf.json` (en la raíz del proyecto Angular):
```json
{
  "/api": {
    "target": "http://localhost:5131",
    "secure": false,
    "changeOrigin": true,
    "logLevel": "debug"
  }
}
```
`package.json`:
```json
"start": "ng serve --open --proxy-config proxy.conf.json"
```
`src/app/app.config.ts`:
```ts
{ provide: API_BASE, useValue: '' } // usa el proxy
```
Ejecutar:
```bash
npm start
```

3) **Tema Angular Material** (`src/styles.css`) — deja un solo import
```css
/* Claro */
@import '@angular/material/prebuilt-themes/indigo-pink.css';
/* @import '@angular/material/prebuilt-themes/deeppurple-amber.css'; */

/* Oscuro */
/* @import '@angular/material/prebuilt-themes/pink-bluegrey.css'; */
/* @import '@angular/material/prebuilt-themes/purple-green.css'; */
```

4) **Polyfills (si ves NG0908 / pantalla en blanco)**
`src/polyfills.ts`:
```ts
import 'zone.js';
```
`angular.json` → `build.options`:
```json
"polyfills": ["src/polyfills.ts"]
```

5) **Rutas incluidas**
- `/persons` – Lista, `/persons/new`, `/persons/:id`
- `/items` – Lista, `/items/new`, `/items/:id`
- `/orders/create` – **Realizar Orden**

6) **Build de producción**
```bash
npm run build
# genera dist/order-frontend
```

---

## 🔗 Contratos API (DTOs que consume el front)

### Crear orden (request)
```json
{
  "personId": 1,
  "lines": [
    { "itemId": 1, "quantity": 2 },
    { "itemId": 2, "quantity": 1, "price": 99.50 }
  ]
}
```

### Leer orden (response)
```json
{
  "id": 10,
  "number": 20250001,
  "personId": 1,
  "personName": "Ada Lovelace",
  "createdAt": "2025-09-05T16:40:00Z",
  "lines": [
    { "id": 1, "itemId": 1, "itemName": "Teclado", "quantity": 2, "price": 25, "total": 50 },
    { "id": 2, "itemId": 2, "itemName": "Mouse",   "quantity": 1, "price": 99.5, "total": 99.5 }
  ],
  "grandTotal": 149.5
}
```

---

## 🧪 Flujo de prueba (E2E)
1. Backend arriba y `GET /api/Person`, `GET /api/Items` responden (aunque sea `[]`).
2. Front en `http://localhost:4200`.
3. Crear **Cliente** e **Items**.
4. Realizar orden en `/orders/create` y confirmar respuesta.

---

## 🧰 Troubleshooting
- **Pantalla en blanco + NG0908:** falta Zone.js (ver polyfills).
- **ERR_CONNECTION_REFUSED:** backend apagado o `API_BASE`/proxy mal configurado.
- **CORS:** usar proxy en dev o habilitar CORS en backend.
- **Deprecation `browserTarget`:** usar `buildTarget` en `angular.json`.
- **Tema Material no aplica:** revisa el `@import` en `styles.css` y evita duplicados.
