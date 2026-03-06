# PipesApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**pipesGet**](PipesApi.md#pipesget) | **GET** /pipes |  |
| [**pipesIdDelete**](PipesApi.md#pipesiddelete) | **DELETE** /pipes/{id} |  |
| [**pipesIdPut**](PipesApi.md#pipesidput) | **PUT** /pipes/{id} |  |
| [**pipesPost**](PipesApi.md#pipespost) | **POST** /pipes |  |
| [**pipesRead**](PipesApi.md#pipesread) | **GET** /pipes/{id} |  |



## pipesGet

> PipeReadAllResponse pipesGet()



### Example

```ts
import {
  Configuration,
  PipesApi,
} from '';
import type { PipesGetRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new PipesApi(config);

  try {
    const data = await api.pipesGet();
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


## pipesIdDelete

> pipesIdDelete(id)



### Example

```ts
import {
  Configuration,
  PipesApi,
} from '';
import type { PipesIdDeleteRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new PipesApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies PipesIdDeleteRequest;

  try {
    const data = await api.pipesIdDelete(body);
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


## pipesIdPut

> PipeUpdateResponse pipesIdPut(id, pipeUpdateModel)



### Example

```ts
import {
  Configuration,
  PipesApi,
} from '';
import type { PipesIdPutRequest } from '';

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
  } satisfies PipesIdPutRequest;

  try {
    const data = await api.pipesIdPut(body);
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


## pipesPost

> PipeCreateResponse pipesPost(pipeCreateRequest)



### Example

```ts
import {
  Configuration,
  PipesApi,
} from '';
import type { PipesPostRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new PipesApi(config);

  const body = {
    // PipeCreateRequest
    pipeCreateRequest: ...,
  } satisfies PipesPostRequest;

  try {
    const data = await api.pipesPost(body);
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

