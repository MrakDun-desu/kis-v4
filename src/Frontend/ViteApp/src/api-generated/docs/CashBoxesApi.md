# CashBoxesApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**cashBoxesRead**](CashBoxesApi.md#cashboxesread) | **GET** /cashboxes/{id} |  |
| [**cashboxesGet**](CashBoxesApi.md#cashboxesget) | **GET** /cashboxes |  |
| [**cashboxesIdDelete**](CashBoxesApi.md#cashboxesiddelete) | **DELETE** /cashboxes/{id} |  |
| [**cashboxesIdPut**](CashBoxesApi.md#cashboxesidput) | **PUT** /cashboxes/{id} |  |
| [**cashboxesPost**](CashBoxesApi.md#cashboxespost) | **POST** /cashboxes |  |



## cashBoxesRead

> CashBoxReadResponse cashBoxesRead(id)



### Example

```ts
import {
  Configuration,
  CashBoxesApi,
} from '';
import type { CashBoxesReadRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new CashBoxesApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies CashBoxesReadRequest;

  try {
    const data = await api.cashBoxesRead(body);
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

[**CashBoxReadResponse**](CashBoxReadResponse.md)

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


## cashboxesGet

> CashBoxReadAllResponse cashboxesGet()



### Example

```ts
import {
  Configuration,
  CashBoxesApi,
} from '';
import type { CashboxesGetRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new CashBoxesApi(config);

  try {
    const data = await api.cashboxesGet();
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

[**CashBoxReadAllResponse**](CashBoxReadAllResponse.md)

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


## cashboxesIdDelete

> cashboxesIdDelete(id)



### Example

```ts
import {
  Configuration,
  CashBoxesApi,
} from '';
import type { CashboxesIdDeleteRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new CashBoxesApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies CashboxesIdDeleteRequest;

  try {
    const data = await api.cashboxesIdDelete(body);
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


## cashboxesIdPut

> CashBoxUpdateResponse cashboxesIdPut(id, cashBoxUpdateRequestModel)



### Example

```ts
import {
  Configuration,
  CashBoxesApi,
} from '';
import type { CashboxesIdPutRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new CashBoxesApi(config);

  const body = {
    // number
    id: 8.14,
    // CashBoxUpdateRequestModel
    cashBoxUpdateRequestModel: ...,
  } satisfies CashboxesIdPutRequest;

  try {
    const data = await api.cashboxesIdPut(body);
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
| **cashBoxUpdateRequestModel** | [CashBoxUpdateRequestModel](CashBoxUpdateRequestModel.md) |  | |

### Return type

[**CashBoxUpdateResponse**](CashBoxUpdateResponse.md)

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


## cashboxesPost

> CashBoxCreateResponse cashboxesPost(name)



### Example

```ts
import {
  Configuration,
  CashBoxesApi,
} from '';
import type { CashboxesPostRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new CashBoxesApi(config);

  const body = {
    // string
    name: name_example,
  } satisfies CashboxesPostRequest;

  try {
    const data = await api.cashboxesPost(body);
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
| **name** | `string` |  | [Defaults to `&#39;Kasa Kachna&#39;`] |

### Return type

[**CashBoxCreateResponse**](CashBoxCreateResponse.md)

### Authorization

[oidc](../README.md#oidc)

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: `application/json`, `application/problem+json`


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | Created |  -  |
| **400** | Bad Request |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

