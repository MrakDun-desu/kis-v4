
# StoreItemReadResponse


## Properties

Name | Type
------------ | -------------
`id` | number
`name` | string
`unitName` | string
`isContainerItem` | boolean
`currentCost` | string
`categories` | [Array&lt;CategoryModel&gt;](CategoryModel.md)
`costs` | [Array&lt;CostModel&gt;](CostModel.md)

## Example

```typescript
import type { StoreItemReadResponse } from ''

// TODO: Update the object below with actual values
const example = {
  "id": null,
  "name": null,
  "unitName": null,
  "isContainerItem": null,
  "currentCost": null,
  "categories": null,
  "costs": null,
} satisfies StoreItemReadResponse

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as StoreItemReadResponse
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


