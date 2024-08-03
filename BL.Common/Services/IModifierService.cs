using KisV4.Common.Models;

namespace KisV4.BL.Common.Services;

public interface IModifierService {
    public int Create(ModifierCreateModel createModel);
    public ModifierReadModel? Read(int id);
    public bool Update(int id, ModifierUpdateModel updateModel);
    public bool Delete(int id);
}
