# PipesApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**pipesCreate**](PipesApi.md#pipescreate) | **POST** /pipes |  |
| [**pipesDelete**](PipesApi.md#pipesdelete) | **DELETE** /pipes/{id} |  |
| [**pipesRead**](PipesApi.md#pipesread) | **GET** /pipes/{id} |  |
| [**pipesReadAll**](PipesApi.md#pipesreadall) | **GET** /pipes |  |
| [**pipesUpdate**](PipesApi.md#pipesupdate) | **PUT** /pipes/{id} |  |



## pipesCreate

> PipeCreateResponse pipesCreate(pipeCreateRequest)



### Example

```ts
import {
  Configuration,
  PipesApi,
} from '';
import type { PipesCreateRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new PipesApi(config);

  const body = {
    // PipeCreateRequest
    pipeCreateRequest: ...,
  } satisfies PipesCreateRequest;

  try {
    const data = await api.pipesCreate(body);
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
| **pipeCreateRequest** | [PipeCreateRequest](PipeCreateRequest.md) |  | |

### Return type

[**PipeCreateResponse**](PipeCreateResponse.md)

### Authorization

[oidc](../README.md#oidc)

### HTTP request headers

- **Content-Type**: `application/json`
- **Accept**: `application/json`, `application/problem+json`


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | Created |  -  |
| **400** | Bad Request |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


## pipesDelete

> pipesDelete(id)



### Example

```ts
import {
  Configuration,
  PipesApi,
} from '';
import type { PipesDeleteRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new PipesApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies PipesDeleteRequest;

  try {
    const data = await api.pipesDelete(body);
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

`void` (Empty response body)

### Authorization

[oidc](../README.md#oidc)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **204** | No Content |  -  |
| **404** | Not Found |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


## pipesRead

> PipeReadResponse pipesRead(id)



### Example

```ts
import {
  Configuration,
  PipesApi,
} from '';
import type { PipesReadRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new PipesApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies PipesReadRequest;

  try {
    const data = await api.pipesRead(body);
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

[**PipeReadResponse**](PipeReadResponse.md)

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


## pipesReadAll

> PipeReadAllResponse pipesReadAll()



### Example

```ts
import {
  Configuration,
  PipesApi,
} from '';
import type { PipesReadAllRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new PipesApi(config);

  try {
    const data = await api.pipesReadAll();
    console.log(data);
  } catch (error) {
    console.error(error);
  }
}

// Run the test
example().catch(console.error);
```

### Parameters

This endpoint does not need any parameter.

### Return type

[**PipeReadAllResponse**](PipeReadAllResponse.md)

### Authorization

[oidc](../README.md#oidc)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: `application/json`


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


## pipesUpdate

> PipeUpdateResponse pipesUpdate(id, pipeUpdateModel)



### Example

```ts
import {
  Configuration,
  PipesApi,
} from '';
import type { PipesUpdateRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new PipesApi(config);

  const body = {
    // number
    id: 8.14,
    // PipeUpdateModel
    pipeUpdateModel: ...,
  } satisfies PipesUpdateRequest;

  try {
    const data = await api.pipesUpdate(body);
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
| **pipeUpdateModel** | [PipeUpdateModel](PipeUpdateModel.md) |  | |

### Return type

[**PipeUpdateResponse**](PipeUpdateResponse.md)

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

