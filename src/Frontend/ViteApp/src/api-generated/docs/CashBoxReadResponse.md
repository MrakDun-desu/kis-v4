
# CashBoxReadResponse


## Properties

Name | Type
------------ | -------------
`id` | number
`name` | string
`salesTransactions` | [AccountTransactionReadAllResponse](AccountTransactionReadAllResponse.md)
`donationsTransactions` | [AccountTransactionReadAllResponse](AccountTransactionReadAllResponse.md)
`stockTakings` | Array&lt;Date&gt;

## Example

```typescript
import type { CashBoxReadResponse } from ''

// TODO: Update the object below with actual values
const example = {
  "id": null,
  "name": null,
  "salesTransactions": null,
  "donationsTransactions": null,
  "stockTakings": null,
} satisfies CashBoxReadResponse

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as CashBoxReadResponse
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


