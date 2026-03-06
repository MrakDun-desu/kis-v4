
# SaleItemCreateRequest


## Properties

Name | Type
------------ | -------------
`name` | string
`image` | string
`marginPercent` | string
`marginStatic` | string
`prestigeAmount` | string
`printType` | [PrintType](PrintType.md)
`categoryIds` | Array&lt;number&gt;
`modifierIds` | Array&lt;number&gt;

## Example

```typescript
import type { SaleItemCreateRequest } from ''

// TODO: Update the object below with actual values
const example = {
  "name": null,
  "image": null,
  "marginPercent": null,
  "marginStatic": null,
  "prestigeAmount": null,
  "printType": null,
  "categoryIds": null,
  "modifierIds": null,
} satisfies SaleItemCreateRequest

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as SaleItemCreateRequest
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


