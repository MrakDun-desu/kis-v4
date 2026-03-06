# StoresApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**storesCreate**](StoresApi.md#storescreate) | **POST** /stores |  |
| [**storesDelete**](StoresApi.md#storesdelete) | **DELETE** /stores/{id} |  |
| [**storesRead**](StoresApi.md#storesread) | **GET** /stores/{id} |  |
| [**storesReadAll**](StoresApi.md#storesreadall) | **GET** /stores |  |
| [**storesUpdate**](StoresApi.md#storesupdate) | **PUT** /stores/{id} |  |



## storesCreate

> StoreCreateResponse storesCreate(storeCreateRequest)



### Example

```ts
import {
  Configuration,
  StoresApi,
} from '';
import type { StoresCreateRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new StoresApi(config);

  const body = {
    // StoreCreateRequest
    storeCreateRequest: ...,
  } satisfies StoresCreateRequest;

  try {
    const data = await api.storesCreate(body);
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
| **storeCreateRequest** | [StoreCreateRequest](StoreCreateRequest.md) |  | |

### Return type

[**StoreCreateResponse**](StoreCreateResponse.md)

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


## storesDelete

> storesDelete(id)



### Example

```ts
import {
  Configuration,
  StoresApi,
} from '';
import type { StoresDeleteRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new StoresApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies StoresDeleteRequest;

  try {
    const data = await api.storesDelete(body);
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


## storesRead

> StoreReadResponse storesRead(id)



### Example

```ts
import {
  Configuration,
  StoresApi,
} from '';
import type { StoresReadRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new StoresApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies StoresReadRequest;

  try {
    const data = await api.storesRead(body);
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

[**StoreReadResponse**](StoreReadResponse.md)

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


## storesReadAll

> StoreReadAllResponse storesReadAll()



### Example

```ts
import {
  Configuration,
  StoresApi,
} from '';
import type { StoresReadAllRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new StoresApi(config);

  try {
    const data = await api.storesReadAll();
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

[**StoreReadAllResponse**](StoreReadAllResponse.md)

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


## storesUpdate

> StoreUpdateResponse storesUpdate(id, storeUpdateRequest)



### Example

```ts
import {
  Configuration,
  StoresApi,
} from '';
import type { StoresUpdateRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new StoresApi(config);

  const body = {
    // number
    id: 8.14,
    // StoreUpdateRequest
    storeUpdateRequest: ...,
  } satisfies StoresUpdateRequest;

  try {
    const data = await api.storesUpdate(body);
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
| **storeUpdateRequest** | [StoreUpdateRequest](StoreUpdateRequest.md) |  | |

### Return type

[**StoreUpdateResponse**](StoreUpdateResponse.md)

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

