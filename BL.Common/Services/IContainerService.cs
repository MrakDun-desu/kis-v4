using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface IContainerService
{
    public OneOf<Page<ContainerReadAllModel>, Dictionary<string, string[]>> ReadAll(
        int? page,
        int? pageSize,
        bool? deleted,
        int? pipeId);

    public OneOf<ContainerReadAllModel, Dictionary<string, string[]>> Create(
        ContainerCreateModel createModel,
        string userName);

    public OneOf<Success, NotFound, Dictionary<string, string[]>> Update(ContainerUpdateModel updateModel);
    public OneOf<Success, NotFound> Delete(int id, string userName);
}