using KisV4.Common.Models;

namespace KisV4.BL.Common.Services;

public interface IContainerService {
    public int Create(ContainerCreateModel createModel);
    public List<ContainerReadAllModel> ReadAll();
    public bool Update(int id, ContainerUpdateModel updateModel);
    public bool Delete(int id);
}
