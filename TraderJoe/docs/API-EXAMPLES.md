# API Testing Examples

Ready-to-use request bodies for trying the API through the Scalar UI.

> **Enum values** (serialized as strings)
> `side`: `"Buy"` or `"Sell"`
> `orderType`: `"Limit"`
> `status`: `"Accepted"` or `"Rejected"`
> `source`: `"Api"` or `"Auto"`

---

## 1. Check prices are flowing first

`GET /api/prices/AAPL`

Prices update roughly twice a second. Note the `currentMarketPrice` value —
you'll use it to submit a trade that gets **accepted** (within 0.8% of it).

Other symbols available:
`GOOGL`, `MSFT`, `AMZN`, `TSLA`, `NVDA`, `EURUSD`, `GBPUSD`, `BTCUSD`, `ETHUSD`

---

## 2. Submit a trade that gets ACCEPTED

`POST /api/trades`

First read the current price from step 1, then use a price within 0.8% of it.
Example assumes AAPL is trading around ~150:

```json
{
  "symbol": "AAPL",
  "price": 150.00,
  "quantity": 10,
  "side": "Buy",
  "orderType": "Limit"
}
```

Expected: `status: "Accepted"`, `rejectionReason: null`.

---

## 3. Submit a trade that gets REJECTED (price deviation)

`POST /api/trades`

A price far from the current market is rejected by the deviation rule:

```json
{
  "symbol": "AAPL",
  "price": 9999.00,
  "quantity": 10,
  "side": "Buy",
  "orderType": "Limit"
}
```

Expected: `status: "Rejected"`, reason mentions price deviation exceeds 0.8%.

---

## 4. Submit a trade that gets REJECTED (max quantity)

`POST /api/trades`

```json
{
  "symbol": "AAPL",
  "price": 150.00,
  "quantity": 999999,
  "side": "Buy",
  "orderType": "Limit"
}
```

Expected: `status: "Rejected"`, reason mentions quantity exceeds the maximum.

---

## 5. Submit a trade that gets REJECTED (lower-bound validation)

`POST /api/trades`

Zero or negative price/quantity is rejected before any rule check:

```json
{
  "symbol": "AAPL",
  "price": 0,
  "quantity": 10,
  "side": "Buy",
  "orderType": "Limit"
}
```

Expected: `400 Bad Request` (caught by model validation on the request).

---

## 6. View trade history (all requests, accepted and rejected)

`GET /api/trades/history`

With filters:

- By symbol: `GET /api/trades/history?symbol=AAPL`
- By status: `GET /api/trades/history?status=Rejected`
- By date range: `GET /api/trades/history?from=2026-06-05T00:00:00&to=2026-06-06T00:00:00`
- Combined: `GET /api/trades/history?symbol=AAPL&status=Accepted`

---

## 7. View accepted orders for a symbol

`GET /api/orders/AAPL`

Returns only **accepted** orders. After the app has run for a little while,
this will also contain **auto-generated** orders (look for `source: "Auto"`)
produced by the spread-based auto-trading engine.

---

## 8. Get current trading rules

`GET /api/rules`

Returns the active rule set. If none has been set yet, sensible defaults are
returned (max deviation 0.8%, duplicate check off, whitelist off).

---

## 9. Update trading rules at runtime

`PUT /api/rules`

Changes take effect immediately — no restart. Example enabling the symbol
whitelist and the duplicate-order check:

```json
{
  "maxNotionalValue": 1000000,
  "maxQuantityPerOrder": 10000,
  "maxPriceDeviationPercent": 0.8,
  "isDuplicateOrderIdCheckEnabled": true,
  "isSymbolWhitelistEnabled": true,
  "symbolWhitelist": ["AAPL", "GOOGL", "MSFT"],
  "autoTradingSpreadThresholdPercent": 0.5
}
```

After this, a trade for a symbol **not** in the whitelist (e.g. `TSLA`) will be
rejected — try it to confirm the runtime update took effect.

---

## 10. Test idempotency (duplicate-order check)

First enable the duplicate check (step 9 above, `isDuplicateOrderIdCheckEnabled: true`).

Then submit the **same** request twice, both with the same `idempotencyKey`:

```json
{
  "idempotencyKey": "550e8400-e29b-41d4-a716-446655440000",
  "symbol": "AAPL",
  "price": 150.00,
  "quantity": 10,
  "side": "Buy",
  "orderType": "Limit"
}
```

The second submission returns the **original** result rather than creating a
second trade request.

---
