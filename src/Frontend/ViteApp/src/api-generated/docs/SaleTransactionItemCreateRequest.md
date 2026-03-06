
# SaleTransactionItemCreateRequest


## Properties

Name | Type
------------ | -------------
`amount` | number
`saleItemId` | number
`modifications` | [Array&lt;ModificationCreateRequest&gt;](ModificationCreateRequest.md)

## Example

```typescript
import type { SaleTransactionItemCreateRequest } from ''

// TODO: Update the object below with actual values
const example = {
  "amount": null,
  "saleItemId": null,
  "modifications": null,
} satisfies SaleTransactionItemCreateRequest

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as SaleTransactionItemCreateRequest
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


