# ContainerChangesApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**containerChangesGet**](ContainerChangesApi.md#containerchangesget) | **GET** /container-changes |  |
| [**containerChangesPost**](ContainerChangesApi.md#containerchangespost) | **POST** /container-changes |  |



## containerChangesGet

> ContainerChangeReadAllResponse containerChangesGet(containerId)



### Example

```ts
import {
  Configuration,
  ContainerChangesApi,
} from '';
import type { ContainerChangesGetRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new ContainerChangesApi(config);

  const body = {
    // number
    containerId: 8.14,
  } satisfies ContainerChangesGetRequest;

  try {
    const data = await api.containerChangesGet(body);
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


## containerChangesPost

> ContainerChangeCreateResponse containerChangesPost(containerChangeCreateRequest)



### Example

```ts
import {
  Configuration,
  ContainerChangesApi,
} from '';
import type { ContainerChangesPostRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new ContainerChangesApi(config);

  const body = {
    // ContainerChangeCreateRequest
    containerChangeCreateRequest: ...,
  } satisfies ContainerChangesPostRequest;

  try {
    const data = await api.containerChangesPost(body);
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

