# StoreItemsApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**storeItemsCreate**](StoreItemsApi.md#storeitemscreate) | **POST** /store-items |  |
| [**storeItemsDelete**](StoreItemsApi.md#storeitemsdelete) | **DELETE** /store-items/{id} |  |
| [**storeItemsRead**](StoreItemsApi.md#storeitemsread) | **GET** /store-items/{id} |  |
| [**storeItemsReadAll**](StoreItemsApi.md#storeitemsreadall) | **GET** /store-items |  |
| [**storeItemsUpdate**](StoreItemsApi.md#storeitemsupdate) | **PUT** /store-items/{id} |  |



## storeItemsCreate

> StoreItemCreateResponse storeItemsCreate(storeItemCreateRequest)



### Example

```ts
import {
  Configuration,
  StoreItemsApi,
} from '';
import type { StoreItemsCreateRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new StoreItemsApi(config);

  const body = {
    // StoreItemCreateRequest
    storeItemCreateRequest: ...,
  } satisfies StoreItemsCreateRequest;

  try {
    const data = await api.storeItemsCreate(body);
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
| **storeItemCreateRequest** | [StoreItemCreateRequest](StoreItemCreateRequest.md) |  | |

### Return type

[**StoreItemCreateResponse**](StoreItemCreateResponse.md)

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


## storeItemsDelete

> storeItemsDelete(id)



### Example

```ts
import {
  Configuration,
  StoreItemsApi,
} from '';
import type { StoreItemsDeleteRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new StoreItemsApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies StoreItemsDeleteRequest;

  try {
    const data = await api.storeItemsDelete(body);
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


## storeItemsRead

> StoreItemReadResponse storeItemsRead(id)



### Example

```ts
import {
  Configuration,
  StoreItemsApi,
} from '';
import type { StoreItemsReadRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new StoreItemsApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies StoreItemsReadRequest;

  try {
    const data = await api.storeItemsRead(body);
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

[**StoreItemReadResponse**](StoreItemReadResponse.md)

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


## storeItemsReadAll

> StoreItemReadAllResponse storeItemsReadAll(name, isContainerItem, categoryId, page, pageSize)



### Example

```ts
import {
  Configuration,
  StoreItemsApi,
} from '';
import type { StoreItemsReadAllRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new StoreItemsApi(config);

  const body = {
    // string (optional)
    name: name_example,
    // boolean (optional)
    isContainerItem: true,
    // number (optional)
    categoryId: 8.14,
    // number (optional)
    page: 8.14,
    // number (optional)
    pageSize: 8.14,
  } satisfies StoreItemsReadAllRequest;

  try {
    const data = await api.storeItemsReadAll(body);
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
| **isContainerItem** | `boolean` |  | [Optional] [Defaults to `undefined`] |
| **categoryId** | `number` |  | [Optional] [Defaults to `undefined`] |
| **page** | `number` |  | [Optional] [Defaults to `undefined`] |
| **pageSize** | `number` |  | [Optional] [Defaults to `undefined`] |

### Return type

[**StoreItemReadAllResponse**](StoreItemReadAllResponse.md)

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


## storeItemsUpdate

> StoreItemUpdateResponse storeItemsUpdate(id, storeItemUpdateModel)



### Example

```ts
import {
  Configuration,
  StoreItemsApi,
} from '';
import type { StoreItemsUpdateRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new StoreItemsApi(config);

  const body = {
    // number
    id: 8.14,
    // StoreItemUpdateModel
    storeItemUpdateModel: ...,
  } satisfies StoreItemsUpdateRequest;

  try {
    const data = await api.storeItemsUpdate(body);
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
| **storeItemUpdateModel** | [StoreItemUpdateModel](StoreItemUpdateModel.md) |  | |

### Return type

[**StoreItemUpdateResponse**](StoreItemUpdateResponse.md)

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

