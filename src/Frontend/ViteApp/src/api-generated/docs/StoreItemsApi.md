# StoreItemsApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**storeItemsGet**](StoreItemsApi.md#storeitemsget) | **GET** /store-items |  |
| [**storeItemsIdDelete**](StoreItemsApi.md#storeitemsiddelete) | **DELETE** /store-items/{id} |  |
| [**storeItemsIdPut**](StoreItemsApi.md#storeitemsidput) | **PUT** /store-items/{id} |  |
| [**storeItemsPost**](StoreItemsApi.md#storeitemspost) | **POST** /store-items |  |
| [**storeItemsRead**](StoreItemsApi.md#storeitemsread) | **GET** /store-items/{id} |  |



## storeItemsGet

> StoreItemReadAllResponse storeItemsGet(name, isContainerItem, categoryId, page, pageSize)



### Example

```ts
import {
  Configuration,
  StoreItemsApi,
} from '';
import type { StoreItemsGetRequest } from '';

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
  } satisfies StoreItemsGetRequest;

  try {
    const data = await api.storeItemsGet(body);
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


## storeItemsIdDelete

> storeItemsIdDelete(id)



### Example

```ts
import {
  Configuration,
  StoreItemsApi,
} from '';
import type { StoreItemsIdDeleteRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new StoreItemsApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies StoreItemsIdDeleteRequest;

  try {
    const data = await api.storeItemsIdDelete(body);
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


## storeItemsIdPut

> StoreItemUpdateResponse storeItemsIdPut(id, storeItemUpdateModel)



### Example

```ts
import {
  Configuration,
  StoreItemsApi,
} from '';
import type { StoreItemsIdPutRequest } from '';

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
  } satisfies StoreItemsIdPutRequest;

  try {
    const data = await api.storeItemsIdPut(body);
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


## storeItemsPost

> StoreItemCreateResponse storeItemsPost(storeItemCreateRequest)



### Example

```ts
import {
  Configuration,
  StoreItemsApi,
} from '';
import type { StoreItemsPostRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new StoreItemsApi(config);

  const body = {
    // StoreItemCreateRequest
    storeItemCreateRequest: ...,
  } satisfies StoreItemsPostRequest;

  try {
    const data = await api.storeItemsPost(body);
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

