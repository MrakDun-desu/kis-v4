using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface IModifierService {
    OneOf<ModifierDetailModel, Dictionary<string, string[]>> Create(ModifierCreateModel createModel);
    OneOf<ModifierDetailModel, NotFound> Read(int id);
    OneOf<ModifierDetailModel, NotFound, Dictionary<string, string[]>> Update(int id, ModifierCreateModel updateModel);
    OneOf<ModifierDetailModel, NotFound> Delete(int id);
}
