
# AccountTransactionReadAllResponse


## Properties

Name | Type
------------ | -------------
`accountId` | number
`from` | Date
`to` | Date
`total` | string
`data` | [Array&lt;AccountTransactionModel&gt;](AccountTransactionModel.md)
`meta` | [PageMeta](PageMeta.md)

## Example

```typescript
import type { AccountTransactionReadAllResponse } from ''

// TODO: Update the object below with actual values
const example = {
  "accountId": null,
  "from": null,
  "to": null,
  "total": null,
  "data": null,
  "meta": null,
} satisfies AccountTransactionReadAllResponse

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as AccountTransactionReadAllResponse
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


