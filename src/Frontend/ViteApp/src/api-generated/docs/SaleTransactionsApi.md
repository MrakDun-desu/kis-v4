# SaleTransactionsApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**saleTransactionsCheckPrice**](SaleTransactionsApi.md#saletransactionscheckprice) | **POST** /sale-transactions/check-price |  |
| [**saleTransactionsClose**](SaleTransactionsApi.md#saletransactionsclose) | **POST** /sale-transactions/{id}/close |  |
| [**saleTransactionsCreate**](SaleTransactionsApi.md#saletransactionscreate) | **POST** /sale-transactions |  |
| [**saleTransactionsDelete**](SaleTransactionsApi.md#saletransactionsdelete) | **DELETE** /sale-transactions/{id} |  |
| [**saleTransactionsOpen**](SaleTransactionsApi.md#saletransactionsopen) | **POST** /sale-transactions/open |  |
| [**saleTransactionsRead**](SaleTransactionsApi.md#saletransactionsread) | **GET** /sale-transactions/{id} |  |
| [**saleTransactionsReadAll**](SaleTransactionsApi.md#saletransactionsreadall) | **GET** /sale-transactions |  |
| [**saleTransactionsUpdate**](SaleTransactionsApi.md#saletransactionsupdate) | **PATCH** /sale-transactions/{id} |  |



## saleTransactionsCheckPrice

> SaleTransactionCheckPriceResponse saleTransactionsCheckPrice(saleTransactionCheckPriceRequest)



### Example

```ts
import {
  Configuration,
  SaleTransactionsApi,
} from '';
import type { SaleTransactionsCheckPriceRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new SaleTransactionsApi(config);

  const body = {
    // SaleTransactionCheckPriceRequest
    saleTransactionCheckPriceRequest: ...,
  } satisfies SaleTransactionsCheckPriceRequest;

  try {
    const data = await api.saleTransactionsCheckPrice(body);
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
| **saleTransactionCheckPriceRequest** | [SaleTransactionCheckPriceRequest](SaleTransactionCheckPriceRequest.md) |  | |

### Return type

[**SaleTransactionCheckPriceResponse**](SaleTransactionCheckPriceResponse.md)

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


## saleTransactionsClose

> SaleTransactionDetailModel saleTransactionsClose(id, saleTransactionCloseRequestModel)



### Example

```ts
import {
  Configuration,
  SaleTransactionsApi,
} from '';
import type { SaleTransactionsCloseRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new SaleTransactionsApi(config);

  const body = {
    // number
    id: 8.14,
    // SaleTransactionCloseRequestModel
    saleTransactionCloseRequestModel: ...,
  } satisfies SaleTransactionsCloseRequest;

  try {
    const data = await api.saleTransactionsClose(body);
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
| **saleTransactionCloseRequestModel** | [SaleTransactionCloseRequestModel](SaleTransactionCloseRequestModel.md) |  | |

### Return type

[**SaleTransactionDetailModel**](SaleTransactionDetailModel.md)

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


## saleTransactionsCreate

> SaleTransactionDetailModel saleTransactionsCreate(saleTransactionCreateRequest)



### Example

```ts
import {
  Configuration,
  SaleTransactionsApi,
} from '';
import type { SaleTransactionsCreateRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new SaleTransactionsApi(config);

  const body = {
    // SaleTransactionCreateRequest
    saleTransactionCreateRequest: ...,
  } satisfies SaleTransactionsCreateRequest;

  try {
    const data = await api.saleTransactionsCreate(body);
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
| **saleTransactionCreateRequest** | [SaleTransactionCreateRequest](SaleTransactionCreateRequest.md) |  | |

### Return type

[**SaleTransactionDetailModel**](SaleTransactionDetailModel.md)

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


## saleTransactionsDelete

> saleTransactionsDelete(id)



### Example

```ts
import {
  Configuration,
  SaleTransactionsApi,
} from '';
import type { SaleTransactionsDeleteRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new SaleTransactionsApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies SaleTransactionsDeleteRequest;

  try {
    const data = await api.saleTransactionsDelete(body);
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


## saleTransactionsOpen

> SaleTransactionDetailModel saleTransactionsOpen(saleTransactionOpenRequest)



### Example

```ts
import {
  Configuration,
  SaleTransactionsApi,
} from '';
import type { SaleTransactionsOpenRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new SaleTransactionsApi(config);

  const body = {
    // SaleTransactionOpenRequest
    saleTransactionOpenRequest: ...,
  } satisfies SaleTransactionsOpenRequest;

  try {
    const data = await api.saleTransactionsOpen(body);
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
| **saleTransactionOpenRequest** | [SaleTransactionOpenRequest](SaleTransactionOpenRequest.md) |  | |

### Return type

[**SaleTransactionDetailModel**](SaleTransactionDetailModel.md)

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


## saleTransactionsRead

> SaleTransactionDetailModel saleTransactionsRead(id)



### Example

```ts
import {
  Configuration,
  SaleTransactionsApi,
} from '';
import type { SaleTransactionsReadRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new SaleTransactionsApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies SaleTransactionsReadRequest;

  try {
    const data = await api.saleTransactionsRead(body);
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

[**SaleTransactionDetailModel**](SaleTransactionDetailModel.md)

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


## saleTransactionsReadAll

> SaleTransactionReadAllResponse saleTransactionsReadAll(from, to, onlySelfCancellable, page, pageSize)



### Example

```ts
import {
  Configuration,
  SaleTransactionsApi,
} from '';
import type { SaleTransactionsReadAllRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new SaleTransactionsApi(config);

  const body = {
    // Date (optional)
    from: 2013-10-20T19:20:30+01:00,
    // Date (optional)
    to: 2013-10-20T19:20:30+01:00,
    // boolean (optional)
    onlySelfCancellable: true,
    // number (optional)
    page: 8.14,
    // number (optional)
    pageSize: 8.14,
  } satisfies SaleTransactionsReadAllRequest;

  try {
    const data = await api.saleTransactionsReadAll(body);
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
| **from** | `Date` |  | [Optional] [Defaults to `undefined`] |
| **to** | `Date` |  | [Optional] [Defaults to `undefined`] |
| **onlySelfCancellable** | `boolean` |  | [Optional] [Defaults to `undefined`] |
| **page** | `number` |  | [Optional] [Defaults to `undefined`] |
| **pageSize** | `number` |  | [Optional] [Defaults to `undefined`] |

### Return type

[**SaleTransactionReadAllResponse**](SaleTransactionReadAllResponse.md)

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


## saleTransactionsUpdate

> SaleTransactionDetailModel saleTransactionsUpdate(id, saleTransactionUpdateRequestModel)



### Example

```ts
import {
  Configuration,
  SaleTransactionsApi,
} from '';
import type { SaleTransactionsUpdateRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new SaleTransactionsApi(config);

  const body = {
    // number
    id: 8.14,
    // SaleTransactionUpdateRequestModel
    saleTransactionUpdateRequestModel: ...,
  } satisfies SaleTransactionsUpdateRequest;

  try {
    const data = await api.saleTransactionsUpdate(body);
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
| **saleTransactionUpdateRequestModel** | [SaleTransactionUpdateRequestModel](SaleTransactionUpdateRequestModel.md) |  | |

### Return type

[**SaleTransactionDetailModel**](SaleTransactionDetailModel.md)

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

