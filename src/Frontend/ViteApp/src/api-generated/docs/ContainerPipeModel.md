
# ContainerPipeModel


## Properties

Name | Type
------------ | -------------
`id` | number
`amount` | string
`state` | [ContainerState](ContainerState.md)
`template` | [ContainerTemplateModel](ContainerTemplateModel.md)

## Example

```typescript
import type { ContainerPipeModel } from ''

// TODO: Update the object below with actual values
const example = {
  "id": null,
  "amount": null,
  "state": null,
  "template": null,
} satisfies ContainerPipeModel

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as ContainerPipeModel
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


