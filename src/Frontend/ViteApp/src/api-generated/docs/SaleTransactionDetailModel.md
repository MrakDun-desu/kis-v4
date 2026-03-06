
# SaleTransactionDetailModel


## Properties

Name | Type
------------ | -------------
`id` | number
`note` | string
`startedAt` | Date
`cancelledAt` | Date
`startedBy` | [UserListModel](UserListModel.md)
`cancelledBy` | [UserListModel](UserListModel.md)
`openedBy` | [UserListModel](UserListModel.md)
`saleTransactionItems` | [Array&lt;SaleTransactionItemModel&gt;](SaleTransactionItemModel.md)
`accountTransactions` | [Array&lt;AccountTransactionModel&gt;](AccountTransactionModel.md)
`storeTransactions` | [Array&lt;StoreTransactionListModel&gt;](StoreTransactionListModel.md)

## Example

```typescript
import type { SaleTransactionDetailModel } from ''

// TODO: Update the object below with actual values
const example = {
  "id": null,
  "note": null,
  "startedAt": null,
  "cancelledAt": null,
  "startedBy": null,
  "cancelledBy": null,
  "openedBy": null,
  "saleTransactionItems": null,
  "accountTransactions": null,
  "storeTransactions": null,
} satisfies SaleTransactionDetailModel

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as SaleTransactionDetailModel
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


