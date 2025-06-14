using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface ICompositionService {
    OneOf<Success, CompositionListModel, Dictionary<string, string[]>> CreateOrUpdate(CompositionCreateModel createModel);
}
