# CategoriesApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**categoriesGet**](CategoriesApi.md#categoriesget) | **GET** /categories |  |
| [**categoriesIdDelete**](CategoriesApi.md#categoriesiddelete) | **DELETE** /categories/{id} |  |
| [**categoriesIdPut**](CategoriesApi.md#categoriesidput) | **PUT** /categories/{id} |  |
| [**categoriesPost**](CategoriesApi.md#categoriespost) | **POST** /categories |  |



## categoriesGet

> CategoryReadAllResponse categoriesGet()



### Example

```ts
import {
  Configuration,
  CategoriesApi,
} from '';
import type { CategoriesGetRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new CategoriesApi(config);

  try {
    const data = await api.categoriesGet();
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


## categoriesIdDelete

> categoriesIdDelete(id)



### Example

```ts
import {
  Configuration,
  CategoriesApi,
} from '';
import type { CategoriesIdDeleteRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new CategoriesApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies CategoriesIdDeleteRequest;

  try {
    const data = await api.categoriesIdDelete(body);
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


## categoriesIdPut

> categoriesIdPut(id, categoryUpdateRequestModel)



### Example

```ts
import {
  Configuration,
  CategoriesApi,
} from '';
import type { CategoriesIdPutRequest } from '';

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
  } satisfies CategoriesIdPutRequest;

  try {
    const data = await api.categoriesIdPut(body);
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


## categoriesPost

> CategoryCreateResponse categoriesPost(categoryCreateRequest)



### Example

```ts
import {
  Configuration,
  CategoriesApi,
} from '';
import type { CategoriesPostRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new CategoriesApi(config);

  const body = {
    // CategoryCreateRequest
    categoryCreateRequest: ...,
  } satisfies CategoriesPostRequest;

  try {
    const data = await api.categoriesPost(body);
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

