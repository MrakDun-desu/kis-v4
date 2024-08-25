namespace KisV4.DAL.EF.Entities;

/// <summary>
///     Represents any account, can be a cash-box or user account.
/// </summary>
public abstract record AccountEntity
{
    public int Id { get; init; }
    public bool Deleted { get; set; }

    /// <summary>
    ///     Currency changes that have occured for this account.
    /// </summary>
    public ICollection<CurrencyChangeEntity> CurrencyChanges { get; private set; } =
        new List<CurrencyChangeEntity>();
}