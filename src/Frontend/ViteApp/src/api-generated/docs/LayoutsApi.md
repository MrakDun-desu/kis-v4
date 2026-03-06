# LayoutsApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**layoutsGet**](LayoutsApi.md#layoutsget) | **GET** /layouts |  |
| [**layoutsIdDelete**](LayoutsApi.md#layoutsiddelete) | **DELETE** /layouts/{id} |  |
| [**layoutsIdPut**](LayoutsApi.md#layoutsidput) | **PUT** /layouts/{id} |  |
| [**layoutsPost**](LayoutsApi.md#layoutspost) | **POST** /layouts |  |
| [**layoutsRead**](LayoutsApi.md#layoutsread) | **GET** /layouts/{id} |  |
| [**layoutsTopLevelGet**](LayoutsApi.md#layoutstoplevelget) | **GET** /layouts/top-level |  |



## layoutsGet

> LayoutReadAllResponse layoutsGet(name)



### Example

```ts
import {
  Configuration,
  LayoutsApi,
} from '';
import type { LayoutsGetRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new LayoutsApi(config);

  const body = {
    // string (optional)
    name: name_example,
  } satisfies LayoutsGetRequest;

  try {
    const data = await api.layoutsGet(body);
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


## layoutsIdDelete

> layoutsIdDelete(id)



### Example

```ts
import {
  Configuration,
  LayoutsApi,
} from '';
import type { LayoutsIdDeleteRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new LayoutsApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies LayoutsIdDeleteRequest;

  try {
    const data = await api.layoutsIdDelete(body);
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


## layoutsIdPut

> LayoutUpdateResponse layoutsIdPut(id, layoutUpdateRequestModel, storeId)



### Example

```ts
import {
  Configuration,
  LayoutsApi,
} from '';
import type { LayoutsIdPutRequest } from '';

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
  } satisfies LayoutsIdPutRequest;

  try {
    const data = await api.layoutsIdPut(body);
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


## layoutsPost

> LayoutCreateResponse layoutsPost(layoutCreateRequestModel, storeId)



### Example

```ts
import {
  Configuration,
  LayoutsApi,
} from '';
import type { LayoutsPostRequest } from '';

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
  } satisfies LayoutsPostRequest;

  try {
    const data = await api.layoutsPost(body);
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


## layoutsTopLevelGet

> LayoutReadResponse layoutsTopLevelGet(storeId)



### Example

```ts
import {
  Configuration,
  LayoutsApi,
} from '';
import type { LayoutsTopLevelGetRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new LayoutsApi(config);

  const body = {
    // number (optional)
    storeId: 8.14,
  } satisfies LayoutsTopLevelGetRequest;

  try {
    const data = await api.layoutsTopLevelGet(body);
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

