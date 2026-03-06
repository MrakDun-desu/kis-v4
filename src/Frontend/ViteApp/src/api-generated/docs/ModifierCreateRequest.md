
# ModifierCreateRequest


## Properties

Name | Type
------------ | -------------
`name` | string
`image` | string
`marginPercent` | string
`marginStatic` | string
`prestigeAmount` | string
`categoryIds` | Array&lt;number&gt;
`targetIds` | Array&lt;number&gt;

## Example

```typescript
import type { ModifierCreateRequest } from ''

// TODO: Update the object below with actual values
const example = {
  "name": null,
  "image": null,
  "marginPercent": null,
  "marginStatic": null,
  "prestigeAmount": null,
  "categoryIds": null,
  "targetIds": null,
} satisfies ModifierCreateRequest

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as ModifierCreateRequest
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


