# StoreTransactionsApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**storeTransactionsGet**](StoreTransactionsApi.md#storetransactionsget) | **GET** /store-transactions |  |
| [**storeTransactionsIdDelete**](StoreTransactionsApi.md#storetransactionsiddelete) | **DELETE** /store-transactions/{id} |  |
| [**storeTransactionsPost**](StoreTransactionsApi.md#storetransactionspost) | **POST** /store-transactions |  |
| [**storeTransactionsRead**](StoreTransactionsApi.md#storetransactionsread) | **GET** /store-transactions/{id} |  |



## storeTransactionsGet

> StoreTransactionReadAllResponse storeTransactionsGet(from, to, onlySelfCancellable, page, pageSize)



### Example

```ts
import {
  Configuration,
  StoreTransactionsApi,
} from '';
import type { StoreTransactionsGetRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new StoreTransactionsApi(config);

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
  } satisfies StoreTransactionsGetRequest;

  try {
    const data = await api.storeTransactionsGet(body);
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

[**StoreTransactionReadAllResponse**](StoreTransactionReadAllResponse.md)

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


## storeTransactionsIdDelete

> storeTransactionsIdDelete(id, updateCosts)



### Example

```ts
import {
  Configuration,
  StoreTransactionsApi,
} from '';
import type { StoreTransactionsIdDeleteRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new StoreTransactionsApi(config);

  const body = {
    // number
    id: 8.14,
    // boolean (optional)
    updateCosts: true,
  } satisfies StoreTransactionsIdDeleteRequest;

  try {
    const data = await api.storeTransactionsIdDelete(body);
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
| **updateCosts** | `boolean` |  | [Optional] [Defaults to `undefined`] |

### Return type

`void` (Empty response body)

### Authorization

[oidc](../README.md#oidc)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: `application/problem+json`


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **204** | No Content |  -  |
| **404** | Not Found |  -  |
| **400** | Bad Request |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


## storeTransactionsPost

> StoreTransactionCreateResponse storeTransactionsPost(storeTransactionCreateRequest)



### Example

```ts
import {
  Configuration,
  StoreTransactionsApi,
} from '';
import type { StoreTransactionsPostRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new StoreTransactionsApi(config);

  const body = {
    // StoreTransactionCreateRequest
    storeTransactionCreateRequest: ...,
  } satisfies StoreTransactionsPostRequest;

  try {
    const data = await api.storeTransactionsPost(body);
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
| **storeTransactionCreateRequest** | [StoreTransactionCreateRequest](StoreTransactionCreateRequest.md) |  | |

### Return type

[**StoreTransactionCreateResponse**](StoreTransactionCreateResponse.md)

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


## storeTransactionsRead

> StoreTransactionReadResponse storeTransactionsRead(id)



### Example

```ts
import {
  Configuration,
  StoreTransactionsApi,
} from '';
import type { StoreTransactionsReadRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new StoreTransactionsApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies StoreTransactionsReadRequest;

  try {
    const data = await api.storeTransactionsRead(body);
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

[**StoreTransactionReadResponse**](StoreTransactionReadResponse.md)

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

