using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface IModifierService
{
    public OneOf<ModifierDetailModel, Dictionary<string, string[]>> Create(ModifierCreateModel createModel);
    public OneOf<ModifierDetailModel, NotFound> Read(int id);
    public OneOf<ModifierDetailModel, NotFound, Dictionary<string, string[]>>Update(int id, ModifierCreateModel updateModel);
    public OneOf<ModifierDetailModel, NotFound> Delete(int id);
}