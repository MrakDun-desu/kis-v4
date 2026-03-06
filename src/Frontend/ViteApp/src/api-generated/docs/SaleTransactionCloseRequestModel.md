
# SaleTransactionCloseRequestModel


## Properties

Name | Type
------------ | -------------
`note` | string
`cashBoxId` | number
`customerId` | number
`paidAmount` | string

## Example

```typescript
import type { SaleTransactionCloseRequestModel } from ''

// TODO: Update the object below with actual values
const example = {
  "note": null,
  "cashBoxId": null,
  "customerId": null,
  "paidAmount": null,
} satisfies SaleTransactionCloseRequestModel

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as SaleTransactionCloseRequestModel
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


