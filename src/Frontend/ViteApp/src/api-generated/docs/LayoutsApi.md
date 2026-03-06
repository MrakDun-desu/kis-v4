# LayoutsApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**layoutsCreate**](LayoutsApi.md#layoutscreate) | **POST** /layouts |  |
| [**layoutsDelete**](LayoutsApi.md#layoutsdelete) | **DELETE** /layouts/{id} |  |
| [**layoutsRead**](LayoutsApi.md#layoutsread) | **GET** /layouts/{id} |  |
| [**layoutsReadAll**](LayoutsApi.md#layoutsreadall) | **GET** /layouts |  |
| [**layoutsReadTopLevel**](LayoutsApi.md#layoutsreadtoplevel) | **GET** /layouts/top-level |  |
| [**layoutsUpdate**](LayoutsApi.md#layoutsupdate) | **PUT** /layouts/{id} |  |



## layoutsCreate

> LayoutCreateResponse layoutsCreate(layoutCreateRequestModel, storeId)



### Example

```ts
import {
  Configuration,
  LayoutsApi,
} from '';
import type { LayoutsCreateRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new LayoutsApi(config);

  const body = {
    // LayoutCreateRequestModel
    layoutCreateRequestModel: ...,
    // number (optional)
    storeId: 8.14,
  } satisfies LayoutsCreateRequest;

  try {
    const data = await api.layoutsCreate(body);
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
| **layoutCreateRequestModel** | [LayoutCreateRequestModel](LayoutCreateRequestModel.md) |  | |
| **storeId** | `number` |  | [Optional] [Defaults to `undefined`] |

### Return type

[**LayoutCreateResponse**](LayoutCreateResponse.md)

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


## layoutsDelete

> layoutsDelete(id)



### Example

```ts
import {
  Configuration,
  LayoutsApi,
} from '';
import type { LayoutsDeleteRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new LayoutsApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies LayoutsDeleteRequest;

  try {
    const data = await api.layoutsDelete(body);
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


## layoutsRead

> LayoutReadResponse layoutsRead(id, storeId)



### Example

```ts
import {
  Configuration,
  LayoutsApi,
} from '';
import type { LayoutsReadRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new LayoutsApi(config);

  const body = {
    // number
    id: 8.14,
    // number (optional)
    storeId: 8.14,
  } satisfies LayoutsReadRequest;

  try {
    const data = await api.layoutsRead(body);
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
| **storeId** | `number` |  | [Optional] [Defaults to `undefined`] |

### Return type

[**LayoutReadResponse**](LayoutReadResponse.md)

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


## layoutsReadAll

> LayoutReadAllResponse layoutsReadAll(name)



### Example

```ts
import {
  Configuration,
  LayoutsApi,
} from '';
import type { LayoutsReadAllRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new LayoutsApi(config);

  const body = {
    // string (optional)
    name: name_example,
  } satisfies LayoutsReadAllRequest;

  try {
    const data = await api.layoutsReadAll(body);
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
| **name** | `string` |  | [Optional] [Defaults to `undefined`] |

### Return type

[**LayoutReadAllResponse**](LayoutReadAllResponse.md)

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


## layoutsReadTopLevel

> LayoutReadResponse layoutsReadTopLevel(storeId)



### Example

```ts
import {
  Configuration,
  LayoutsApi,
} from '';
import type { LayoutsReadTopLevelRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new LayoutsApi(config);

  const body = {
    // number (optional)
    storeId: 8.14,
  } satisfies LayoutsReadTopLevelRequest;

  try {
    const data = await api.layoutsReadTopLevel(body);
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

### Return type

[**LayoutReadResponse**](LayoutReadResponse.md)

### Authorization

[oidc](../README.md#oidc)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: `application/json`, `application/problem+json`


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **404** | Not Found |  -  |
| **400** | Bad Request |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


## layoutsUpdate

> LayoutUpdateResponse layoutsUpdate(id, layoutUpdateRequestModel, storeId)



### Example

```ts
import {
  Configuration,
  LayoutsApi,
} from '';
import type { LayoutsUpdateRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new LayoutsApi(config);

  const body = {
    // number
    id: 8.14,
    // LayoutUpdateRequestModel
    layoutUpdateRequestModel: ...,
    // number (optional)
    storeId: 8.14,
  } satisfies LayoutsUpdateRequest;

  try {
    const data = await api.layoutsUpdate(body);
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
| **layoutUpdateRequestModel** | [LayoutUpdateRequestModel](LayoutUpdateRequestModel.md) |  | |
| **storeId** | `number` |  | [Optional] [Defaults to `undefined`] |

### Return type

[**LayoutUpdateResponse**](LayoutUpdateResponse.md)

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

