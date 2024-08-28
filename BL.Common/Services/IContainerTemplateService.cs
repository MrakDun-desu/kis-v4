using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface IContainerTemplateService
{
    public OneOf<ICollection<ContainerTemplateReadAllModel>, Dictionary<string, string[]>> ReadAll(
        bool? deleted,
        int? containedItemId);

    public OneOf<ContainerTemplateReadAllModel, Dictionary<string, string[]>> Create(
        ContainerTemplateCreateModel createModel);

    public OneOf<Success, NotFound, Dictionary<string, string[]>> Update(ContainerTemplateUpdateModel updateModel);
    public OneOf<Success, NotFound> Delete(int id);
}