
# AccountTransactionModel


## Properties

Name | Type
------------ | -------------
`amount` | string
`saleTransactionId` | number
`timestamp` | Date
`type` | [AccountTransactionType](AccountTransactionType.md)

## Example

```typescript
import type { AccountTransactionModel } from ''

// TODO: Update the object below with actual values
const example = {
  "amount": null,
  "saleTransactionId": null,
  "timestamp": null,
  "type": null,
} satisfies AccountTransactionModel

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as AccountTransactionModel
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


