# CompositionsApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**compositionsPut**](CompositionsApi.md#compositionsput) | **PUT** /compositions |  |
| [**compositionsReadAll**](CompositionsApi.md#compositionsreadall) | **GET** /compositions |  |



## compositionsPut

> compositionsPut(compositionPutRequest)



### Example

```ts
import {
  Configuration,
  CompositionsApi,
} from '';
import type { CompositionsPutRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new CompositionsApi(config);

  const body = {
    // CompositionPutRequest
    compositionPutRequest: ...,
  } satisfies CompositionsPutRequest;

  try {
    const data = await api.compositionsPut(body);
    console.log(data);
  } catch (error) {
    console.error(error);
  }
}

// Run the test
example().catch(console.error);
```

### Parameters


| Name | Type | Description  | Notes |
|------------- | ------------- | ------------- | -------------|
| **compositionPutRequest** | [CompositionPutRequest](CompositionPutRequest.md) |  | |

### Return type

`void` (Empty response body)

### Authorization

[oidc](../README.md#oidc)

### HTTP request headers

- **Content-Type**: `application/json`
- **Accept**: `application/problem+json`


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **400** | Bad Request |  -  |
| **204** | No Content |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


## compositionsReadAll

> CompositionReadAllResponse compositionsReadAll(compositeId)



### Example

```ts
import {
  Configuration,
  CompositionsApi,
} from '';
import type { CompositionsReadAllRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new CompositionsApi(config);

  const body = {
    // number
    compositeId: 8.14,
  } satisfies CompositionsReadAllRequest;

  try {
    const data = await api.compositionsReadAll(body);
    console.log(data);
  } catch (error) {
    console.error(error);
  }
}

// Run the test
example().catch(console.error);
```

### Parameters


| Name | Type | Description  | Notes |
|------------- | ------------- | ------------- | -------------|
| **compositeId** | `number` |  | [Defaults to `undefined`] |

### Return type

[**CompositionReadAllResponse**](CompositionReadAllResponse.md)

### Authorization

[oidc](../README.md#oidc)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: `application/json`, `application/problem+json`


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **400** | Bad Request |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

