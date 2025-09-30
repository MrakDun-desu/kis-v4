using KisV4.Common.Models;
using OneOf;
using OneOf.Types;

namespace KisV4.BL.Common.Services;

public interface IUserService {
    int CreateOrGetId(string userName);
    OneOf<Page<UserListModel>, Dictionary<string, string[]>> ReadAll(int? page, int? pageSize, bool? deleted);
    OneOf<UserDetailModel, NotFound, Dictionary<string, string[]>> Read(int id);
}
