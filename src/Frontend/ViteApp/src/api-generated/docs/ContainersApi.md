# ContainersApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**containersGet**](ContainersApi.md#containersget) | **GET** /containers |  |
| [**containersIdGet**](ContainersApi.md#containersidget) | **GET** /containers/{id} |  |
| [**containersIdOperatorGet**](ContainersApi.md#containersidoperatorget) | **GET** /containers/{id}/operator |  |
| [**containersIdPut**](ContainersApi.md#containersidput) | **PUT** /containers/{id} |  |
| [**containersPost**](ContainersApi.md#containerspost) | **POST** /containers |  |



## containersGet

> ContainerReadAllResponse containersGet(storeId, templateId, pipeId, includeUnusable, page, pageSize)



### Example

```ts
import {
  Configuration,
  ContainersApi,
} from '';
import type { ContainersGetRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new ContainersApi(config);

  const body = {
    // number (optional)
    storeId: 8.14,
    // number (optional)
    templateId: 8.14,
    // number (optional)
    pipeId: 8.14,
    // boolean (optional)
    includeUnusable: true,
    // number (optional)
    page: 8.14,
    // number (optional)
    pageSize: 8.14,
  } satisfies ContainersGetRequest;

  try {
    const data = await api.containersGet(body);
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
| **storeId** | `number` |  | [Optional] [Defaults to `undefined`] |
| **templateId** | `number` |  | [Optional] [Defaults to `undefined`] |
| **pipeId** | `number` |  | [Optional] [Defaults to `undefined`] |
| **includeUnusable** | `boolean` |  | [Optional] [Defaults to `undefined`] |
| **page** | `number` |  | [Optional] [Defaults to `undefined`] |
| **pageSize** | `number` |  | [Optional] [Defaults to `undefined`] |

### Return type

[**ContainerReadAllResponse**](ContainerReadAllResponse.md)

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


## containersIdGet

> ContainerReadResponse containersIdGet(id)



### Example

```ts
import {
  Configuration,
  ContainersApi,
} from '';
import type { ContainersIdGetRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new ContainersApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies ContainersIdGetRequest;

  try {
    const data = await api.containersIdGet(body);
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
| **id** | `number` |  | [Defaults to `undefined`] |

### Return type

[**ContainerReadResponse**](ContainerReadResponse.md)

### Authorization

[oidc](../README.md#oidc)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: `application/json`


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **404** | Not Found |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


## containersIdOperatorGet

> ContainerOperatorReadResponse containersIdOperatorGet(id)



### Example

```ts
import {
  Configuration,
  ContainersApi,
} from '';
import type { ContainersIdOperatorGetRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new ContainersApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies ContainersIdOperatorGetRequest;

  try {
    const data = await api.containersIdOperatorGet(body);
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
| **id** | `number` |  | [Defaults to `undefined`] |

### Return type

[**ContainerOperatorReadResponse**](ContainerOperatorReadResponse.md)

### Authorization

[oidc](../README.md#oidc)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: `application/json`


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **404** | Not Found |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


## containersIdPut

> ContainerUpdateResponse containersIdPut(id, containerUpdateModel)



### Example

```ts
import {
  Configuration,
  ContainersApi,
} from '';
import type { ContainersIdPutRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new ContainersApi(config);

  const body = {
    // number
    id: 8.14,
    // ContainerUpdateModel
    containerUpdateModel: ...,
  } satisfies ContainersIdPutRequest;

  try {
    const data = await api.containersIdPut(body);
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
| **id** | `number` |  | [Defaults to `undefined`] |
| **containerUpdateModel** | [ContainerUpdateModel](ContainerUpdateModel.md) |  | |

### Return type

[**ContainerUpdateResponse**](ContainerUpdateResponse.md)

### Authorization

[oidc](../README.md#oidc)

### HTTP request headers

- **Content-Type**: `application/json`
- **Accept**: `application/json`, `application/problem+json`


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **404** | Not Found |  -  |
| **400** | Bad Request |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


## containersPost

> ContainerCreateResponse containersPost(containerCreateRequest)



### Example

```ts
import {
  Configuration,
  ContainersApi,
} from '';
import type { ContainersPostRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new ContainersApi(config);

  const body = {
    // ContainerCreateRequest
    containerCreateRequest: ...,
  } satisfies ContainersPostRequest;

  try {
    const data = await api.containersPost(body);
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
| **containerCreateRequest** | [ContainerCreateRequest](ContainerCreateRequest.md) |  | |

### Return type

[**ContainerCreateResponse**](ContainerCreateResponse.md)

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

