using KisV4.Common.Models;
using OneOf;

namespace KisV4.BL.Common.Services;

public interface ICurrencyChangeService {
    OneOf<Page<CurrencyChangeListModel>, Dictionary<string, string[]>> ReadAll(
        int? page,
        int? pageSize,
        int? accountId,
        bool? cancelled,
        DateTimeOffset? startDate,
        DateTimeOffset? endDate
    );
}
