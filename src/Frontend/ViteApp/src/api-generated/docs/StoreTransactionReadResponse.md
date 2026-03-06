
# StoreTransactionReadResponse


## Properties

Name | Type
------------ | -------------
`id` | number
`note` | string
`startedAt` | Date
`cancelledAt` | Date
`startedBy` | [UserListModel](UserListModel.md)
`cancelledBy` | [UserListModel](UserListModel.md)
`reason` | [TransactionReason](TransactionReason.md)
`saleTransactionId` | number
`storeTransactionItems` | [Array&lt;StoreTransactionItemModel&gt;](StoreTransactionItemModel.md)

## Example

```typescript
import type { StoreTransactionReadResponse } from ''

// TODO: Update the object below with actual values
const example = {
  "id": null,
  "note": null,
  "startedAt": null,
  "cancelledAt": null,
  "startedBy": null,
  "cancelledBy": null,
  "reason": null,
  "saleTransactionId": null,
  "storeTransactionItems": null,
} satisfies StoreTransactionReadResponse

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as StoreTransactionReadResponse
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


