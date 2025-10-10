using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface IPipeService {
    PipeListModel Create(PipeCreateModel createModel);
    List<PipeListModel> ReadAll();
    OneOf<PipeListModel, NotFound> Update(int id, PipeCreateModel updateModel);
    OneOf<Success, NotFound, string> Delete(int id);
}
