using KisV4.Common.Models;

namespace KisV4.BL.Common.Services;

public interface IPipeService
{
    public int Create(PipeCreateModel createModel);
    public List<PipeListModel> ReadAll();
    public bool Update(int id, PipeCreateModel updateModel);
    public bool Delete(int id);
}