namespace KisV4.DAL.EF.Entities;

/// <summary>
/// Represents any account, can be a cashbox or user account.
/// </summary>
public abstract record AccountEntity {
    public int Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public bool Deleted { get; set; }

    /// <summary>
    /// Currency changes that have occured for this account.
    /// </summary>
    public virtual ICollection<CurrencyChangeEntity> CurrencyChanges { get; private set; } =
        new List<CurrencyChangeEntity>();
}
