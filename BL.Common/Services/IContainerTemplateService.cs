using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface IContainerTemplateService
{
    public OneOf<ICollection<ContainerTemplateListModel>, Dictionary<string, string[]>> ReadAll(
        bool? deleted,
        int? containedItemId);

    public OneOf<ContainerTemplateListModel, Dictionary<string, string[]>> Create(
        ContainerTemplateCreateModel createModel);

    public OneOf<ContainerTemplateListModel, NotFound, Dictionary<string, string[]>> Update(int id, ContainerTemplateCreateModel updateModel);
    public OneOf<ContainerTemplateListModel, NotFound> Delete(int id);
}