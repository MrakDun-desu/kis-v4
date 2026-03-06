
# ContainerChangeCreateRequest


## Properties

Name | Type
------------ | -------------
`newAmount` | string
`newState` | [ContainerState](ContainerState.md)
`containerId` | number

## Example

```typescript
import type { ContainerChangeCreateRequest } from ''

// TODO: Update the object below with actual values
const example = {
  "newAmount": null,
  "newState": null,
  "containerId": null,
} satisfies ContainerChangeCreateRequest

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as ContainerChangeCreateRequest
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


