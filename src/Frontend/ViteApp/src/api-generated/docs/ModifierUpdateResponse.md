
# ModifierUpdateResponse


## Properties

Name | Type
------------ | -------------
`id` | number
`name` | string
`image` | string
`marginPercent` | string
`marginStatic` | string
`prestigeAmount` | string
`categories` | [Array&lt;CategoryModel&gt;](CategoryModel.md)
`targets` | [Array&lt;SaleItemListModel&gt;](SaleItemListModel.md)

## Example

```typescript
import type { ModifierUpdateResponse } from ''

// TODO: Update the object below with actual values
const example = {
  "id": null,
  "name": null,
  "image": null,
  "marginPercent": null,
  "marginStatic": null,
  "prestigeAmount": null,
  "categories": null,
  "targets": null,
} satisfies ModifierUpdateResponse

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as ModifierUpdateResponse
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


