# ContainerChangesApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**containerChangesCreate**](ContainerChangesApi.md#containerchangescreate) | **POST** /container-changes |  |
| [**containerChangesReadAll**](ContainerChangesApi.md#containerchangesreadall) | **GET** /container-changes |  |



## containerChangesCreate

> ContainerChangeCreateResponse containerChangesCreate(containerChangeCreateRequest)



### Example

```ts
import {
  Configuration,
  ContainerChangesApi,
} from '';
import type { ContainerChangesCreateRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new ContainerChangesApi(config);

  const body = {
    // ContainerChangeCreateRequest
    containerChangeCreateRequest: ...,
  } satisfies ContainerChangesCreateRequest;

  try {
    const data = await api.containerChangesCreate(body);
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
| **containerChangeCreateRequest** | [ContainerChangeCreateRequest](ContainerChangeCreateRequest.md) |  | |

### Return type

[**ContainerChangeCreateResponse**](ContainerChangeCreateResponse.md)

### Authorization

[oidc](../README.md#oidc)

### HTTP request headers

- **Content-Type**: `application/json`
- **Accept**: `application/json`, `application/problem+json`


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **400** | Bad Request |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


## containerChangesReadAll

> ContainerChangeReadAllResponse containerChangesReadAll(containerId)



### Example

```ts
import {
  Configuration,
  ContainerChangesApi,
} from '';
import type { ContainerChangesReadAllRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new ContainerChangesApi(config);

  const body = {
    // number
    containerId: 8.14,
  } satisfies ContainerChangesReadAllRequest;

  try {
    const data = await api.containerChangesReadAll(body);
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
| **containerId** | `number` |  | [Defaults to `undefined`] |

### Return type

[**ContainerChangeReadAllResponse**](ContainerChangeReadAllResponse.md)

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

