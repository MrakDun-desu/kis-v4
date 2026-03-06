# ModifiersApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**modifiersCreate**](ModifiersApi.md#modifierscreate) | **POST** /modifiers |  |
| [**modifiersDelete**](ModifiersApi.md#modifiersdelete) | **DELETE** /modifiers/{id} |  |
| [**modifiersRead**](ModifiersApi.md#modifiersread) | **GET** /modifers/{id} |  |
| [**modifiersReadAll**](ModifiersApi.md#modifiersreadall) | **GET** /modifiers |  |
| [**modifiersUpdate**](ModifiersApi.md#modifiersupdate) | **PUT** /modifiers/{id} |  |



## modifiersCreate

> ModifierCreateResponse modifiersCreate(modifierCreateRequest)



### Example

```ts
import {
  Configuration,
  ModifiersApi,
} from '';
import type { ModifiersCreateRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new ModifiersApi(config);

  const body = {
    // ModifierCreateRequest
    modifierCreateRequest: ...,
  } satisfies ModifiersCreateRequest;

  try {
    const data = await api.modifiersCreate(body);
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
| **modifierCreateRequest** | [ModifierCreateRequest](ModifierCreateRequest.md) |  | |

### Return type

[**ModifierCreateResponse**](ModifierCreateResponse.md)

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


## modifiersDelete

> modifiersDelete(id)



### Example

```ts
import {
  Configuration,
  ModifiersApi,
} from '';
import type { ModifiersDeleteRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new ModifiersApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies ModifiersDeleteRequest;

  try {
    const data = await api.modifiersDelete(body);
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


## modifiersRead

> ModifierReadResponse modifiersRead(id)



### Example

```ts
import {
  Configuration,
  ModifiersApi,
} from '';
import type { ModifiersReadRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new ModifiersApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies ModifiersReadRequest;

  try {
    const data = await api.modifiersRead(body);
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

[**ModifierReadResponse**](ModifierReadResponse.md)

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


## modifiersReadAll

> ModifierReadAllResponse modifiersReadAll(name, categoryId, targetId, page, pageSize)



### Example

```ts
import {
  Configuration,
  ModifiersApi,
} from '';
import type { ModifiersReadAllRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new ModifiersApi(config);

  const body = {
    // string (optional)
    name: name_example,
    // number (optional)
    categoryId: 8.14,
    // number (optional)
    targetId: 8.14,
    // number (optional)
    page: 8.14,
    // number (optional)
    pageSize: 8.14,
  } satisfies ModifiersReadAllRequest;

  try {
    const data = await api.modifiersReadAll(body);
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
| **targetId** | `number` |  | [Optional] [Defaults to `undefined`] |
| **page** | `number` |  | [Optional] [Defaults to `undefined`] |
| **pageSize** | `number` |  | [Optional] [Defaults to `undefined`] |

### Return type

[**ModifierReadAllResponse**](ModifierReadAllResponse.md)

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


## modifiersUpdate

> ModifierUpdateResponse modifiersUpdate(id, modifierUpdateModel)



### Example

```ts
import {
  Configuration,
  ModifiersApi,
} from '';
import type { ModifiersUpdateRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new ModifiersApi(config);

  const body = {
    // number
    id: 8.14,
    // ModifierUpdateModel
    modifierUpdateModel: ...,
  } satisfies ModifiersUpdateRequest;

  try {
    const data = await api.modifiersUpdate(body);
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
| **modifierUpdateModel** | [ModifierUpdateModel](ModifierUpdateModel.md) |  | |

### Return type

[**ModifierUpdateResponse**](ModifierUpdateResponse.md)

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

