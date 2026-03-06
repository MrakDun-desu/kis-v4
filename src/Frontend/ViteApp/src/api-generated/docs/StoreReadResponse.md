
# StoreReadResponse


## Properties

Name | Type
------------ | -------------
`id` | number
`name` | string
`storeItemAmounts` | [StoreItemAmountReadAllResponse](StoreItemAmountReadAllResponse.md)
`containers` | [ContainerReadAllResponse](ContainerReadAllResponse.md)

## Example

```typescript
import type { StoreReadResponse } from ''

// TODO: Update the object below with actual values
const example = {
  "id": null,
  "name": null,
  "storeItemAmounts": null,
  "containers": null,
} satisfies StoreReadResponse

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as StoreReadResponse
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


