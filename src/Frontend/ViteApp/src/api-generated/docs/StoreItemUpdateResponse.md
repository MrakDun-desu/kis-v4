
# StoreItemUpdateResponse


## Properties

Name | Type
------------ | -------------
`id` | number
`name` | string
`unitName` | string
`isContainerItem` | boolean
`currentCost` | string
`categories` | [Array&lt;CategoryModel&gt;](CategoryModel.md)

## Example

```typescript
import type { StoreItemUpdateResponse } from ''

// TODO: Update the object below with actual values
const example = {
  "id": null,
  "name": null,
  "unitName": null,
  "isContainerItem": null,
  "currentCost": null,
  "categories": null,
} satisfies StoreItemUpdateResponse

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as StoreItemUpdateResponse
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


