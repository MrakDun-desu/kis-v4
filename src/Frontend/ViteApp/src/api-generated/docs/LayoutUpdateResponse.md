
# LayoutUpdateResponse


## Properties

Name | Type
------------ | -------------
`id` | number
`name` | string
`image` | string
`topLevel` | boolean
`layoutItems` | [Array&lt;LayoutItemModel&gt;](LayoutItemModel.md)

## Example

```typescript
import type { LayoutUpdateResponse } from ''

// TODO: Update the object below with actual values
const example = {
  "id": null,
  "name": null,
  "image": null,
  "topLevel": null,
  "layoutItems": null,
} satisfies LayoutUpdateResponse

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as LayoutUpdateResponse
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


