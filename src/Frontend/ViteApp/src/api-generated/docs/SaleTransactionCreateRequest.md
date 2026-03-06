
# SaleTransactionCreateRequest


## Properties

Name | Type
------------ | -------------
`note` | string
`storeId` | number
`cashBoxId` | number
`customerId` | number
`paidAmount` | string
`saleTransactionItems` | [Array&lt;SaleTransactionItemCreateRequest&gt;](SaleTransactionItemCreateRequest.md)

## Example

```typescript
import type { SaleTransactionCreateRequest } from ''

// TODO: Update the object below with actual values
const example = {
  "note": null,
  "storeId": null,
  "cashBoxId": null,
  "customerId": null,
  "paidAmount": null,
  "saleTransactionItems": null,
} satisfies SaleTransactionCreateRequest

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as SaleTransactionCreateRequest
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


