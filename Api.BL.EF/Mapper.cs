using Api.DAL.EF.Entities;
using KisV4.Api.Common.Models.Cashbox;
using Riok.Mapperly.Abstractions;

namespace Api.BL.EF;

[Mapper]
public partial class Mapper {
    public partial CashboxEntity ToEntity(CashboxCreateModel model);
    public partial CashboxDetailModel? ToModel(CashboxEntity? entity);
    public partial IEnumerable<CashboxListModel> ToModelEnumerable(IEnumerable<CashboxEntity> entity);
}
