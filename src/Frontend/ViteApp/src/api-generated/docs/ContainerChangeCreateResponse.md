
# ContainerChangeCreateResponse


## Properties

Name | Type
------------ | -------------
`newState` | [ContainerState](ContainerState.md)
`timestamp` | Date
`containerId` | number
`user` | [UserListModel](UserListModel.md)

## Example

```typescript
import type { ContainerChangeCreateResponse } from ''

// TODO: Update the object below with actual values
const example = {
  "newState": null,
  "timestamp": null,
  "containerId": null,
  "user": null,
} satisfies ContainerChangeCreateResponse

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as ContainerChangeCreateResponse
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


