# SaleItemsApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**saleItemsCreate**](SaleItemsApi.md#saleitemscreate) | **POST** /sale-items |  |
| [**saleItemsDelete**](SaleItemsApi.md#saleitemsdelete) | **DELETE** /sale-items/{id} |  |
| [**saleItemsRead**](SaleItemsApi.md#saleitemsread) | **GET** /sale-items/{id} |  |
| [**saleItemsReadAll**](SaleItemsApi.md#saleitemsreadall) | **GET** /sale-items |  |
| [**saleItemsUpdate**](SaleItemsApi.md#saleitemsupdate) | **PUT** /sale-items/{id} |  |



## saleItemsCreate

> SaleItemCreateResponse saleItemsCreate(saleItemCreateRequest)



### Example

```ts
import {
  Configuration,
  SaleItemsApi,
} from '';
import type { SaleItemsCreateRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new SaleItemsApi(config);

  const body = {
    // SaleItemCreateRequest
    saleItemCreateRequest: ...,
  } satisfies SaleItemsCreateRequest;

  try {
    const data = await api.saleItemsCreate(body);
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
| **saleItemCreateRequest** | [SaleItemCreateRequest](SaleItemCreateRequest.md) |  | |

### Return type

[**SaleItemCreateResponse**](SaleItemCreateResponse.md)

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


## saleItemsDelete

> saleItemsDelete(id)



### Example

```ts
import {
  Configuration,
  SaleItemsApi,
} from '';
import type { SaleItemsDeleteRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new SaleItemsApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies SaleItemsDeleteRequest;

  try {
    const data = await api.saleItemsDelete(body);
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


## saleItemsRead

> SaleItemReadResponse saleItemsRead(id)



### Example

```ts
import {
  Configuration,
  SaleItemsApi,
} from '';
import type { SaleItemsReadRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new SaleItemsApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies SaleItemsReadRequest;

  try {
    const data = await api.saleItemsRead(body);
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

[**SaleItemReadResponse**](SaleItemReadResponse.md)

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


## saleItemsReadAll

> SaleItemReadAllResponse saleItemsReadAll(name, categoryId, page, pageSize)



### Example

```ts
import {
  Configuration,
  SaleItemsApi,
} from '';
import type { SaleItemsReadAllRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new SaleItemsApi(config);

  const body = {
    // string (optional)
    name: name_example,
    // number (optional)
    categoryId: 8.14,
    // number (optional)
    page: 8.14,
    // number (optional)
    pageSize: 8.14,
  } satisfies SaleItemsReadAllRequest;

  try {
    const data = await api.saleItemsReadAll(body);
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
| **categoryId** | `number` |  | [Optional] [Defaults to `undefined`] |
| **page** | `number` |  | [Optional] [Defaults to `undefined`] |
| **pageSize** | `number` |  | [Optional] [Defaults to `undefined`] |

### Return type

[**SaleItemReadAllResponse**](SaleItemReadAllResponse.md)

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


## saleItemsUpdate

> SaleItemUpdateResponse saleItemsUpdate(id, saleItemUpdateModel)



### Example

```ts
import {
  Configuration,
  SaleItemsApi,
} from '';
import type { SaleItemsUpdateRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new SaleItemsApi(config);

  const body = {
    // number
    id: 8.14,
    // SaleItemUpdateModel
    saleItemUpdateModel: ...,
  } satisfies SaleItemsUpdateRequest;

  try {
    const data = await api.saleItemsUpdate(body);
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
| **saleItemUpdateModel** | [SaleItemUpdateModel](SaleItemUpdateModel.md) |  | |

### Return type

[**SaleItemUpdateResponse**](SaleItemUpdateResponse.md)

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

