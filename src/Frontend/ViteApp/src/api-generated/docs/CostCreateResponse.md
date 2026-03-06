
# CostCreateResponse


## Properties

Name | Type
------------ | -------------
`amount` | string
`timestamp` | Date
`description` | string
`storeItemId` | number
`user` | [UserListModel](UserListModel.md)

## Example

```typescript
import type { CostCreateResponse } from ''

// TODO: Update the object below with actual values
const example = {
  "amount": null,
  "timestamp": null,
  "description": null,
  "storeItemId": null,
  "user": null,
} satisfies CostCreateResponse

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as CostCreateResponse
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


