using Api.DAL.EF.Entities;
using KisV4.Api.Common.Models.CashBox;
using Riok.Mapperly.Abstractions;

namespace Api.BL.EF;

[Mapper]
public partial class Mapper {
    public partial CashBoxEntity ToEntity(CashboxCreateModel model);
    public partial CashBoxDetailModel? ToModel(CashBoxEntity? entity);
    public partial IEnumerable<CashboxListModel> ToModelEnumerable(IEnumerable<CashBoxEntity> entity);
}
