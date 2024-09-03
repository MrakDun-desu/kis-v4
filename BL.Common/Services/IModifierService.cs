using KisV4.Common.Models;

namespace KisV4.BL.Common.Services;

public interface IModifierService
{
    public int Create(ModifierCreateModel createModel);
    public ModifierDetailModel? Read(int id);
    public bool Update(int id, ModifierCreateModel updateModel);
    public bool Delete(int id);
}