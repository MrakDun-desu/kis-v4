
# StoreTransactionCreateRequest


## Properties

Name | Type
------------ | -------------
`note` | string
`storeTransactionItems` | [Array&lt;StoreTransactionItemCreateRequest&gt;](StoreTransactionItemCreateRequest.md)
`reason` | [TransactionReason](TransactionReason.md)
`storeId` | number
`sourceStoreId` | number
`updateCosts` | boolean

## Example

```typescript
import type { StoreTransactionCreateRequest } from ''

// TODO: Update the object below with actual values
const example = {
  "note": null,
  "storeTransactionItems": null,
  "reason": null,
  "storeId": null,
  "sourceStoreId": null,
  "updateCosts": null,
} satisfies StoreTransactionCreateRequest

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as StoreTransactionCreateRequest
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


