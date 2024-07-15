using KisV4.Common.Models;
using KisV4.DAL.EF.Entities;
using Riok.Mapperly.Abstractions;

namespace KisV4.BL.EF;

[Mapper]
public partial class Mapper {
    public partial CashBoxEntity ToEntity(CashBoxCreateModel model);
    public partial CashBoxDetailModel? ToModel(CashBoxEntity? entity);
    public partial IEnumerable<CashBoxListModel> ToModelEnumerable(IEnumerable<CashBoxEntity> entity);

    public partial CurrencyEntity ToEntity(CurrencyCreateModel model);
    public partial IEnumerable<CurrencyModel> ToModelEnumerable(IEnumerable<CurrencyEntity> entity);
}
