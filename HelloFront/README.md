# Order Frontend (Angular 17 + Material)

## Requisitos
- Node 18+
- Angular CLI (`npm i -g @angular/cli`)

## Configuración
1. Edita `src/app/app.config.ts` y ajusta `API_BASE` a la URL de tu backend.
2. Backend esperado:
   - `GET/POST /api/Person`
   - `GET/POST/PUT/DELETE /api/Items`
   - `POST /api/Orders` con `{ personId, lines: [{ itemId, quantity, price? }] }`

## Ejecutar
```bash
npm install
npx ng serve -o
```
