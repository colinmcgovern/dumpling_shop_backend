# How to run

az account clear

az login --use-device-code

dotnet run

az ad sp create-for-rbac --name "dumpling-api-sp"

## Endpoints
 
### `GET /list_orders`
 
Returns all orders.
 
### `POST /insert_order`
 
Creates a new order. The `orderPlaced` timestamp is set server-side via `SYSDATETIME()`.
 
```json
{
  "orderName": "Pork Dumplings",
  "estimatedCompleted": "2026-03-23T15:00:00",
  "isCompleted": false,
  "isPickedUp": false
}
```
 
### `POST /update_order`
 
Updates an existing order. Requires `orderId`.
 
```json
{
  "orderId": 1,
  "orderName": "Pork Dumplings",
  "estimatedCompleted": "2026-03-23T15:00:00",
  "isCompleted": true,
  "isPickedUp": false
}
```