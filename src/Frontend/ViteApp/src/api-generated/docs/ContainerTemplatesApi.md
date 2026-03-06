# ContainerTemplatesApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**containerTemplatesGet**](ContainerTemplatesApi.md#containertemplatesget) | **GET** /container-templates |  |
| [**containerTemplatesIdDelete**](ContainerTemplatesApi.md#containertemplatesiddelete) | **DELETE** /container-templates/{id} |  |
| [**containerTemplatesIdPut**](ContainerTemplatesApi.md#containertemplatesidput) | **PUT** /container-templates/{id} |  |
| [**containerTemplatesPost**](ContainerTemplatesApi.md#containertemplatespost) | **POST** /container-templates |  |



## containerTemplatesGet

> ContainerTemplateReadAllResponse containerTemplatesGet(name, storeItemId, page, pageSize)



### Example

```ts
import {
  Configuration,
  ContainerTemplatesApi,
} from '';
import type { ContainerTemplatesGetRequest } from '';

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
  } satisfies ContainerTemplatesGetRequest;

  try {
    const data = await api.containerTemplatesGet(body);
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


## containerTemplatesIdDelete

> containerTemplatesIdDelete(id)



### Example

```ts
import {
  Configuration,
  ContainerTemplatesApi,
} from '';
import type { ContainerTemplatesIdDeleteRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new ContainerTemplatesApi(config);

  const body = {
    // number
    id: 8.14,
  } satisfies ContainerTemplatesIdDeleteRequest;

  try {
    const data = await api.containerTemplatesIdDelete(body);
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


## containerTemplatesIdPut

> ContainerTemplateUpdateResponse containerTemplatesIdPut(id, containerTemplateUpdateRequestModel)



### Example

```ts
import {
  Configuration,
  ContainerTemplatesApi,
} from '';
import type { ContainerTemplatesIdPutRequest } from '';

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
  } satisfies ContainerTemplatesIdPutRequest;

  try {
    const data = await api.containerTemplatesIdPut(body);
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


## containerTemplatesPost

> ContainerTemplateCreateResponse containerTemplatesPost(containerTemplateCreateRequest)



### Example

```ts
import {
  Configuration,
  ContainerTemplatesApi,
} from '';
import type { ContainerTemplatesPostRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new ContainerTemplatesApi(config);

  const body = {
    // ContainerTemplateCreateRequest
    containerTemplateCreateRequest: ...,
  } satisfies ContainerTemplatesPostRequest;

  try {
    const data = await api.containerTemplatesPost(body);
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

