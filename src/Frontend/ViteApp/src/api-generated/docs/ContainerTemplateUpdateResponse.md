
# ContainerTemplateUpdateResponse


## Properties

Name | Type
------------ | -------------
`id` | number
`name` | string
`amount` | string
`storeItem` | [StoreItemListModel](StoreItemListModel.md)

## Example

```typescript
import type { ContainerTemplateUpdateResponse } from ''

// TODO: Update the object below with actual values
const example = {
  "id": null,
  "name": null,
  "amount": null,
  "storeItem": null,
} satisfies ContainerTemplateUpdateResponse

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as ContainerTemplateUpdateResponse
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


