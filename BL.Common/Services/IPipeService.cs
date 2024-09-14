using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface IPipeService
{
    public PipeListModel Create(PipeCreateModel createModel);
    public List<PipeListModel> ReadAll();
    public OneOf<PipeListModel, NotFound> Update(int id, PipeCreateModel updateModel);
    public OneOf<Success, NotFound, string> Delete(int id);
}