using KisV4.Common.Models;
using KisV4.DAL.EF.Entities;

namespace KisV4.BL.EF.Mapping;

public static class CategoryMapping {
    public static CategoryModel ToModel(this Category source) =>
        new CategoryModel {
            Id = source.Id,
            Name = source.Name
        };
}
