
# SaleTransactionItemModel


## Properties

Name | Type
------------ | -------------
`lineNumber` | number
`amount` | number
`saleItemName` | string
`modifications` | [Array&lt;ModificationModel&gt;](ModificationModel.md)
`basePrice` | string

## Example

```typescript
import type { SaleTransactionItemModel } from ''

// TODO: Update the object below with actual values
const example = {
  "lineNumber": null,
  "amount": null,
  "saleItemName": null,
  "modifications": null,
  "basePrice": null,
} satisfies SaleTransactionItemModel

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as SaleTransactionItemModel
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


