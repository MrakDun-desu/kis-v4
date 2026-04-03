
using System.Text.Json.Serialization;
using KisV4.Common.Enums;

namespace KisV4.Common.Models;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(CashBoxAccountModel), "CashBoxAccount")]
[JsonDerivedType(typeof(UserAccountModel), "UserAccount")]
public abstract record AccountModel {
    public required int Id { get; init; }
    public required AccountType Type { get; init; }
}

public record CashBoxAccountModel : AccountModel {
    public required CashBoxListModel CashBox { get; init; }
}

public record UserAccountModel : AccountModel {
    public required UserListModel User { get; init; }
}
