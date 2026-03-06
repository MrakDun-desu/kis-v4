# ContainerTemplatesApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**containerTemplatesCreate**](ContainerTemplatesApi.md#containertemplatescreate) | **POST** /container-templates |  |
| [**containerTemplatesDelete**](ContainerTemplatesApi.md#containertemplatesdelete) | **DELETE** /container-templates/{id} |  |
| [**containerTemplatesReadAll**](ContainerTemplatesApi.md#containertemplatesreadall) | **GET** /container-templates |  |
| [**containerTemplatesUpdate**](ContainerTemplatesApi.md#containertemplatesupdate) | **PUT** /container-templates/{id} |  |



## containerTemplatesCreate

> ContainerTemplateCreateResponse containerTemplatesCreate(containerTemplateCreateRequest)



### Example

```ts
import {
  Configuration,
  ContainerTemplatesApi,
} from '';
import type { ContainerTemplatesCreateRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new ContainerTemplatesApi(config);

  const body = {
    // ContainerTemplateCreateRequest
    containerTemplateCreateRequest: ...,
  } satisfies ContainerTemplatesCreateRequest;

  try {
    const data = await api.containerTemplatesCreate(body);
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
| **containerTemplateCreateRequest** | [ContainerTemplateCreateRequest](ContainerTemplateCreateRequest.md) |  | |

### Return type

[**ContainerTemplateCreateResponse**](ContainerTemplateCreateResponse.md)

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


## containerTemplatesDelete

> containerTemplatesDelete(id)



### Example

```ts
import {
  Configuration,
  ContainerTemplatesApi,
} from '';
import type { ContainerTemplatesDeleteRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new ContainerTemplatesApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies ContainerTemplatesDeleteRequest;

  try {
    const data = await api.containerTemplatesDelete(body);
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


## containerTemplatesReadAll

> ContainerTemplateReadAllResponse containerTemplatesReadAll(name, storeItemId, page, pageSize)



### Example

```ts
import {
  Configuration,
  ContainerTemplatesApi,
} from '';
import type { ContainerTemplatesReadAllRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new ContainerTemplatesApi(config);

  const body = {
    // string (optional)
    name: name_example,
    // number (optional)
    storeItemId: 8.14,
    // number (optional)
    page: 8.14,
    // number (optional)
    pageSize: 8.14,
  } satisfies ContainerTemplatesReadAllRequest;

  try {
    const data = await api.containerTemplatesReadAll(body);
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
| **storeItemId** | `number` |  | [Optional] [Defaults to `undefined`] |
| **page** | `number` |  | [Optional] [Defaults to `undefined`] |
| **pageSize** | `number` |  | [Optional] [Defaults to `undefined`] |

### Return type

[**ContainerTemplateReadAllResponse**](ContainerTemplateReadAllResponse.md)

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


## containerTemplatesUpdate

> ContainerTemplateUpdateResponse containerTemplatesUpdate(id, containerTemplateUpdateRequestModel)



### Example

```ts
import {
  Configuration,
  ContainerTemplatesApi,
} from '';
import type { ContainerTemplatesUpdateRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new ContainerTemplatesApi(config);

  const body = {
    // number
    id: 8.14,
    // ContainerTemplateUpdateRequestModel
    containerTemplateUpdateRequestModel: ...,
  } satisfies ContainerTemplatesUpdateRequest;

  try {
    const data = await api.containerTemplatesUpdate(body);
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
| **containerTemplateUpdateRequestModel** | [ContainerTemplateUpdateRequestModel](ContainerTemplateUpdateRequestModel.md) |  | |

### Return type

[**ContainerTemplateUpdateResponse**](ContainerTemplateUpdateResponse.md)

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

