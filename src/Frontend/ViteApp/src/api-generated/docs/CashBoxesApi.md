# CashBoxesApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**cashBoxesCreate**](CashBoxesApi.md#cashboxescreate) | **POST** /cashboxes |  |
| [**cashBoxesDelete**](CashBoxesApi.md#cashboxesdelete) | **DELETE** /cashboxes/{id} |  |
| [**cashBoxesRead**](CashBoxesApi.md#cashboxesread) | **GET** /cashboxes/{id} |  |
| [**cashBoxesReadAll**](CashBoxesApi.md#cashboxesreadall) | **GET** /cashboxes |  |
| [**cashBoxesUpdate**](CashBoxesApi.md#cashboxesupdate) | **PUT** /cashboxes/{id} |  |



## cashBoxesCreate

> CashBoxCreateResponse cashBoxesCreate(name)



### Example

```ts
import {
  Configuration,
  CashBoxesApi,
} from '';
import type { CashBoxesCreateRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new CashBoxesApi(config);

  const body = {
    // string
    name: name_example,
  } satisfies CashBoxesCreateRequest;

  try {
    const data = await api.cashBoxesCreate(body);
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


## cashBoxesDelete

> cashBoxesDelete(id)



### Example

```ts
import {
  Configuration,
  CashBoxesApi,
} from '';
import type { CashBoxesDeleteRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new CashBoxesApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies CashBoxesDeleteRequest;

  try {
    const data = await api.cashBoxesDelete(body);
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


## cashBoxesReadAll

> CashBoxReadAllResponse cashBoxesReadAll()



### Example

```ts
import {
  Configuration,
  CashBoxesApi,
} from '';
import type { CashBoxesReadAllRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new CashBoxesApi(config);

  try {
    const data = await api.cashBoxesReadAll();
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


## cashBoxesUpdate

> CashBoxUpdateResponse cashBoxesUpdate(id, cashBoxUpdateRequestModel)



### Example

```ts
import {
  Configuration,
  CashBoxesApi,
} from '';
import type { CashBoxesUpdateRequest } from '';

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
  } satisfies CashBoxesUpdateRequest;

  try {
    const data = await api.cashBoxesUpdate(body);
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

