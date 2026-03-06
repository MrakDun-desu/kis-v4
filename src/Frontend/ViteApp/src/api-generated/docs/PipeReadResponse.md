
# PipeReadResponse


## Properties

Name | Type
------------ | -------------
`id` | number
`name` | string
`containers` | [Array&lt;ContainerPipeModel&gt;](ContainerPipeModel.md)

## Example

```typescript
import type { PipeReadResponse } from ''

// TODO: Update the object below with actual values
const example = {
  "id": null,
  "name": null,
  "containers": null,
} satisfies PipeReadResponse

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as PipeReadResponse
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


