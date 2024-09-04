using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface IContainerService
{
    public OneOf<Page<ContainerListModel>, Dictionary<string, string[]>> ReadAll(
        int? page,
        int? pageSize,
        bool? deleted,
        int? pipeId);

    public OneOf<ContainerListModel, Dictionary<string, string[]>> Create(
        ContainerCreateModel createModel,
        string userName);

    public OneOf<ContainerListModel, NotFound, Dictionary<string, string[]>> Patch(int id, ContainerPatchModel updateModel);
    public OneOf<ContainerListModel, NotFound> Delete(int id, string userName);
}