# StoreItemAmountsApi

All URIs are relative to *http://localhost*

| Method | HTTP request | Description |
|------------- | ------------- | -------------|
| [**storeItemAmountsGet**](StoreItemAmountsApi.md#storeitemamountsget) | **GET** /store-item-amounts |  |



## storeItemAmountsGet

> StoreItemAmountReadAllResponse storeItemAmountsGet(storeId, page, pageSize)



### Example

```ts
import {
  Configuration,
  StoreItemAmountsApi,
} from '';
import type { StoreItemAmountsGetRequest } from '';

async function example() {
  console.log("🚀 Testing  SDK...");
  const config = new Configuration({ 
  });
  const api = new StoreItemAmountsApi(config);

  const body = {
    // number
    storeId: 8.14,
    // number (optional)
    page: 8.14,
    // number (optional)
    pageSize: 8.14,
  } satisfies StoreItemAmountsGetRequest;

  try {
    const data = await api.storeItemAmountsGet(body);
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
| **storeId** | `number` |  | [Defaults to `undefined`] |
| **page** | `number` |  | [Optional] [Defaults to `undefined`] |
| **pageSize** | `number` |  | [Optional] [Defaults to `undefined`] |

### Return type

[**StoreItemAmountReadAllResponse**](StoreItemAmountReadAllResponse.md)

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

