# SaleTransactionsApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**saleTransactionsCheckPricePost**](SaleTransactionsApi.md#saletransactionscheckpricepost) | **POST** /sale-transactions/check-price |  |
| [**saleTransactionsGet**](SaleTransactionsApi.md#saletransactionsget) | **GET** /sale-transactions |  |
| [**saleTransactionsIdClosePost**](SaleTransactionsApi.md#saletransactionsidclosepost) | **POST** /sale-transactions/{id}/close |  |
| [**saleTransactionsIdDelete**](SaleTransactionsApi.md#saletransactionsiddelete) | **DELETE** /sale-transactions/{id} |  |
| [**saleTransactionsIdPatch**](SaleTransactionsApi.md#saletransactionsidpatch) | **PATCH** /sale-transactions/{id} |  |
| [**saleTransactionsOpenPost**](SaleTransactionsApi.md#saletransactionsopenpost) | **POST** /sale-transactions/open |  |
| [**saleTransactionsPost**](SaleTransactionsApi.md#saletransactionspost) | **POST** /sale-transactions |  |
| [**saleTransactionsRead**](SaleTransactionsApi.md#saletransactionsread) | **GET** /sale-transactions/{id} |  |



## saleTransactionsCheckPricePost

> SaleTransactionCheckPriceResponse saleTransactionsCheckPricePost(saleTransactionCheckPriceRequest)



### Example

```ts
import {
  Configuration,
  SaleTransactionsApi,
} from '';
import type { SaleTransactionsCheckPricePostRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new SaleTransactionsApi(config);

  const body = {
    // SaleTransactionCheckPriceRequest
    saleTransactionCheckPriceRequest: ...,
  } satisfies SaleTransactionsCheckPricePostRequest;

  try {
    const data = await api.saleTransactionsCheckPricePost(body);
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


## saleTransactionsGet

> SaleTransactionReadAllResponse saleTransactionsGet(from, to, onlySelfCancellable, page, pageSize)



### Example

```ts
import {
  Configuration,
  SaleTransactionsApi,
} from '';
import type { SaleTransactionsGetRequest } from '';

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
  } satisfies SaleTransactionsGetRequest;

  try {
    const data = await api.saleTransactionsGet(body);
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


## saleTransactionsIdClosePost

> SaleTransactionDetailModel saleTransactionsIdClosePost(id, saleTransactionCloseRequestModel)



### Example

```ts
import {
  Configuration,
  SaleTransactionsApi,
} from '';
import type { SaleTransactionsIdClosePostRequest } from '';

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
  } satisfies SaleTransactionsIdClosePostRequest;

  try {
    const data = await api.saleTransactionsIdClosePost(body);
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


## saleTransactionsIdDelete

> saleTransactionsIdDelete(id)



### Example

```ts
import {
  Configuration,
  SaleTransactionsApi,
} from '';
import type { SaleTransactionsIdDeleteRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new SaleTransactionsApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies SaleTransactionsIdDeleteRequest;

  try {
    const data = await api.saleTransactionsIdDelete(body);
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


## saleTransactionsIdPatch

> SaleTransactionDetailModel saleTransactionsIdPatch(id, saleTransactionUpdateRequestModel)



### Example

```ts
import {
  Configuration,
  SaleTransactionsApi,
} from '';
import type { SaleTransactionsIdPatchRequest } from '';

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
  } satisfies SaleTransactionsIdPatchRequest;

  try {
    const data = await api.saleTransactionsIdPatch(body);
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


## saleTransactionsOpenPost

> SaleTransactionDetailModel saleTransactionsOpenPost(saleTransactionOpenRequest)



### Example

```ts
import {
  Configuration,
  SaleTransactionsApi,
} from '';
import type { SaleTransactionsOpenPostRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new SaleTransactionsApi(config);

  const body = {
    // SaleTransactionOpenRequest
    saleTransactionOpenRequest: ...,
  } satisfies SaleTransactionsOpenPostRequest;

  try {
    const data = await api.saleTransactionsOpenPost(body);
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


## saleTransactionsPost

> SaleTransactionDetailModel saleTransactionsPost(saleTransactionCreateRequest)



### Example

```ts
import {
  Configuration,
  SaleTransactionsApi,
} from '';
import type { SaleTransactionsPostRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new SaleTransactionsApi(config);

  const body = {
    // SaleTransactionCreateRequest
    saleTransactionCreateRequest: ...,
  } satisfies SaleTransactionsPostRequest;

  try {
    const data = await api.saleTransactionsPost(body);
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

