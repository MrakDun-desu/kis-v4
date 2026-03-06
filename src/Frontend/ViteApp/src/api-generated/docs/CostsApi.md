# CostsApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**costsCreate**](CostsApi.md#costscreate) | **POST** /costs |  |



## costsCreate

> CostCreateResponse costsCreate(costCreateRequest)



### Example

```ts
import {
  Configuration,
  CostsApi,
} from '';
import type { CostsCreateRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new CostsApi(config);

  const body = {
    // CostCreateRequest
    costCreateRequest: ...,
  } satisfies CostsCreateRequest;

  try {
    const data = await api.costsCreate(body);
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
| **costCreateRequest** | [CostCreateRequest](CostCreateRequest.md) |  | |

### Return type

[**CostCreateResponse**](CostCreateResponse.md)

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

