# ContainersApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**containersCreate**](ContainersApi.md#containerscreate) | **POST** /containers |  |
| [**containersOperatorRead**](ContainersApi.md#containersoperatorread) | **GET** /containers/{id}/operator |  |
| [**containersRead**](ContainersApi.md#containersread) | **GET** /containers/{id} |  |
| [**containersReadAll**](ContainersApi.md#containersreadall) | **GET** /containers |  |
| [**containersUpdate**](ContainersApi.md#containersupdate) | **PUT** /containers/{id} |  |



## containersCreate

> ContainerCreateResponse containersCreate(containerCreateRequest)



### Example

```ts
import {
  Configuration,
  ContainersApi,
} from '';
import type { ContainersCreateRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new ContainersApi(config);

  const body = {
    // ContainerCreateRequest
    containerCreateRequest: ...,
  } satisfies ContainersCreateRequest;

  try {
    const data = await api.containersCreate(body);
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


## containersOperatorRead

> ContainerOperatorReadResponse containersOperatorRead(id)



### Example

```ts
import {
  Configuration,
  ContainersApi,
} from '';
import type { ContainersOperatorReadRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new ContainersApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies ContainersOperatorReadRequest;

  try {
    const data = await api.containersOperatorRead(body);
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


## containersRead

> ContainerReadResponse containersRead(id)



### Example

```ts
import {
  Configuration,
  ContainersApi,
} from '';
import type { ContainersReadRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new ContainersApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies ContainersReadRequest;

  try {
    const data = await api.containersRead(body);
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


## containersReadAll

> ContainerReadAllResponse containersReadAll(storeId, templateId, pipeId, includeUnusable, page, pageSize)



### Example

```ts
import {
  Configuration,
  ContainersApi,
} from '';
import type { ContainersReadAllRequest } from '';

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
  } satisfies ContainersReadAllRequest;

  try {
    const data = await api.containersReadAll(body);
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


## containersUpdate

> ContainerUpdateResponse containersUpdate(id, containerUpdateModel)



### Example

```ts
import {
  Configuration,
  ContainersApi,
} from '';
import type { ContainersUpdateRequest } from '';

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
  } satisfies ContainersUpdateRequest;

  try {
    const data = await api.containersUpdate(body);
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

