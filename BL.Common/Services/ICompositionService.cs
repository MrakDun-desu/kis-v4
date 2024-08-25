using KisV4.Common.Models;

namespace KisV4.BL.Common.Services;

public interface ICompositionService
{
    public Dictionary<string, string[]>? Create(CompositionCreateModel createModel);
}