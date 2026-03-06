# CategoriesApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**categoriesCreate**](CategoriesApi.md#categoriescreate) | **POST** /categories |  |
| [**categoriesDelete**](CategoriesApi.md#categoriesdelete) | **DELETE** /categories/{id} |  |
| [**categoriesReadAll**](CategoriesApi.md#categoriesreadall) | **GET** /categories |  |
| [**categoriesUpdate**](CategoriesApi.md#categoriesupdate) | **PUT** /categories/{id} |  |



## categoriesCreate

> CategoryCreateResponse categoriesCreate(categoryCreateRequest)



### Example

```ts
import {
  Configuration,
  CategoriesApi,
} from '';
import type { CategoriesCreateRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new CategoriesApi(config);

  const body = {
    // CategoryCreateRequest
    categoryCreateRequest: ...,
  } satisfies CategoriesCreateRequest;

  try {
    const data = await api.categoriesCreate(body);
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
| **categoryCreateRequest** | [CategoryCreateRequest](CategoryCreateRequest.md) |  | |

### Return type

[**CategoryCreateResponse**](CategoryCreateResponse.md)

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


## categoriesDelete

> categoriesDelete(id)



### Example

```ts
import {
  Configuration,
  CategoriesApi,
} from '';
import type { CategoriesDeleteRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new CategoriesApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies CategoriesDeleteRequest;

  try {
    const data = await api.categoriesDelete(body);
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


## categoriesReadAll

> CategoryReadAllResponse categoriesReadAll()



### Example

```ts
import {
  Configuration,
  CategoriesApi,
} from '';
import type { CategoriesReadAllRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new CategoriesApi(config);

  try {
    const data = await api.categoriesReadAll();
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

[**CategoryReadAllResponse**](CategoryReadAllResponse.md)

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


## categoriesUpdate

> categoriesUpdate(id, categoryUpdateRequestModel)



### Example

```ts
import {
  Configuration,
  CategoriesApi,
} from '';
import type { CategoriesUpdateRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new CategoriesApi(config);

  const body = {
    // number
    id: 8.14,
    // CategoryUpdateRequestModel
    categoryUpdateRequestModel: ...,
  } satisfies CategoriesUpdateRequest;

  try {
    const data = await api.categoriesUpdate(body);
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
| **categoryUpdateRequestModel** | [CategoryUpdateRequestModel](CategoryUpdateRequestModel.md) |  | |

### Return type

`void` (Empty response body)

### Authorization

[oidc](../README.md#oidc)

### HTTP request headers

- **Content-Type**: `application/json`
- **Accept**: `application/problem+json`


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **204** | No Content |  -  |
| **404** | Not Found |  -  |
| **400** | Bad Request |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)

