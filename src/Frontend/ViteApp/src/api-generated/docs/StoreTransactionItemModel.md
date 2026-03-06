
# StoreTransactionItemModel


## Properties

Name | Type
------------ | -------------
`itemAmount` | string
`cost` | string
`storeItem` | [StoreItemListModel](StoreItemListModel.md)
`store` | [StoreListModel](StoreListModel.md)
`storeTransactionId` | number

## Example

```typescript
import type { StoreTransactionItemModel } from ''

// TODO: Update the object below with actual values
const example = {
  "itemAmount": null,
  "cost": null,
  "storeItem": null,
  "store": null,
  "storeTransactionId": null,
} satisfies StoreTransactionItemModel

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as StoreTransactionItemModel
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


