using KisV4.Common.Models;

namespace KisV4.BL.Common.Services;

public interface ICompositionService {
    public void Create(CompositionCreateModel createModel);
}
