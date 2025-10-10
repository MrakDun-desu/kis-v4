using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface IContainerService {
    OneOf<Page<ContainerListModel>, Dictionary<string, string[]>> ReadAll(
        int? page,
        int? pageSize,
        bool? deleted,
        int? pipeId);

    OneOf<ContainerListModel, Dictionary<string, string[]>> Create(
        ContainerCreateModel createModel,
        string userName);

    OneOf<ContainerListModel, NotFound, Dictionary<string, string[]>> Patch(int id, ContainerPatchModel updateModel);
    OneOf<ContainerListModel, NotFound> Delete(int id, string userName);
}
