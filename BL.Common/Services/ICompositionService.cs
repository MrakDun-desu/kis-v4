using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface ICompositionService
{
    public OneOf<Success, Dictionary<string, string[]>> Create(CompositionCreateModel createModel);
}