using KisV4.Common.Models;

namespace KisV4.BL.Common;

public interface IPipeService {
    public int Create(PipeCreateModel createModel);
    public List<PipeModel> ReadAll();
    public bool Update(int id, PipeUpdateModel updateModel);
    public bool Delete(int id);
}
