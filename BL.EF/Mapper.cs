using KisV4.Common.Models.CashBox;
using KisV4.Common.Models.Currency;
using KisV4.DAL.EF.Entities;
using Riok.Mapperly.Abstractions;

namespace KisV4.BL.EF;

[Mapper]
public partial class Mapper {
    public partial CashBoxEntity ToEntity(CashboxCreateModel model);
    public partial CashBoxDetailModel? ToModel(CashBoxEntity? entity);
    public partial IEnumerable<CashboxListModel> ToModelEnumerable(IEnumerable<CashBoxEntity> entity);

    public partial CurrencyEntity ToEntity(CurrencyCreateModel model);
    public partial IEnumerable<CurrencyModel> ToModelEnumerable(IEnumerable<CurrencyEntity> entity);
}
