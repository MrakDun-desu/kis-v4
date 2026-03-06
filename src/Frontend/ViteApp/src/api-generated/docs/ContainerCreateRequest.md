
# ContainerCreateRequest


## Properties

Name | Type
------------ | -------------
`templateId` | number
`storeId` | number
`amount` | number
`cost` | string
`updateCosts` | boolean

## Example

```typescript
import type { ContainerCreateRequest } from ''

// TODO: Update the object below with actual values
const example = {
  "templateId": null,
  "storeId": null,
  "amount": null,
  "cost": null,
  "updateCosts": null,
} satisfies ContainerCreateRequest

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as ContainerCreateRequest
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


