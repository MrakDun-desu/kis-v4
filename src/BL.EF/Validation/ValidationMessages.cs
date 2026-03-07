namespace KisV4.BL.EF.Validation;

public static class ValidationMessages {
    // ------------- composable messages -----------
    const string TooLongMessage = "přesahuje maximální povolenou délku";

    // --------------- prop names ------------------
    public const string DateRangePropName = "Období";

    public const string NamePropName = "Jméno";
    public const string UnitNamePropName = "Jednotka";
    public const string NotePropName = "Poznámka";
    public const string DescriptionPropName = "Popis";

    public const string AmountPropName = "Množství";
    public const string CostPropName = "Cena";
    public const string ContainerStatePropName = "Stav kegu";
    public const string LayoutItemsPropName = "Layoutové položky";
    public const string TypePropName = "Typ";
    public const string PagePropName = "Stránka";
    public const string PageSizePropName = "Velikost stránky";
    public const string PaidAmountPropName = "Zaplacená cena";
    public const string SaleTransactionItemsPropName = "Transakční položky";
    public const string StoreTransactionItemsPropName = "Transakční položky";
    public const string TransactionReasonPropName = "Důvod transakce";

    public const string StoreIdPropName = "ID skladu";
    public const string ContainerIdPropName = "ID kegu";
    public const string StoreItemIdPropName = "ID skladové položky";
    public const string CompositeIdPropName = "ID složené položky";
    public const string ContainerTemplateIdPropName = "ID typu kegu";
    public const string PipeIdPropName = "ID pípy";
    public const string CategoryIdPropName = "ID kategorie";
    public const string CashBoxIdPropName = "ID kasy";
    public const string CategoryIdsPropName = "ID kategorií";
    public const string TargetIdsPropName = "ID cílů";
    public const string ModifierIdsPropName = "ID modifikátorů";
    public const string StoreItemIdsPropName = "ID skladových položek";

    // ------------- concrete messages --------------
    public const string BadDateRangeMessage = """
        Počátek specifikovaného období musí být dříve než jeho konec
        """;
    public const string AmountOutOfRangeMessage = """
        Specifikované množství je mimo rozsah
        """;
    public const string AmountTooLowMessage = """
        Specifikované množství je příliš malé
        """;
    public const string CostOutOfRangeMessage = """
        Specifikovaná nákupní cena je mimo rozsah
        """;
    public const string ContainerAmountTooGreatMessage = """
        Nové množství položky v kegu musí být nižší nebo rovné předchozímu množství
        """;
    public const string ContainerInvalidNewStateMessage = """
        Nastavení tohto stavu pro tento keg není možné
        """;
    public const string StoreItemUsedInContainersMessage = """
        Tato skladová položka se používá pro typ kegu
        """;
    public const string ContainerTemplateUsedMessage = """
        Tento typ kegu se používá
        """;
    public const string LayoutItemsNotUniqueMessage = """
        Všechny položky v layoutu musí mít unikátní pozice
        """;
    public const string LayoutItemTargetsNotValidMessage = """
        Všechny položky v layotu musí mít správné odpovídající identifikátory cílů
        """;
    public const string LayoutItemPositionOutOfRangeMessage = """
        Pozice layoutové položky není platná
        """;
    public const string PageOutOfRangeMessage = """
        Specifikované číslo stránky je mimo rozsah
        """;
    public const string PageSizeOutOfRangeMessage = """
        Specifikované číslo stránky je mimo rozsah
        """;
    public const string PaidAmountLessThan0Message = """
        Zaplacená cena musí být větší nebo rovna 0
        """;
    public const string PaidAmountTooLowMessage = """
        Zaplacená cena nestačí na zaplacení této transakce
        """;
    public const string SaleTransactionItemsNotValidCompositesMessage = """
        Všechny transakční položky musí obsahovat validní prodejní položky a modifikátory
        """;
    public const string SaleTransactionItemModifiersNotValidMessage = """
        Aspoň jeden ze specifikovaných modifikátorů není přiřazen správné prodejní položce
        """;
    public const string StoreTransactionItemAmountsNegativeMessage = """
        Kombinace prodejních položek a modifikátorů znamená prodání negativního množství skladové položky
        """;
    public const string CantCreateStoreTransactionAsSaleMessage = """
        Není možné vytvořit samotnou skladovou transakci jako součást prodeje
        """;
    public const string SourceStoreSameAsTargetStoreMessage = """
        Cílový sklad nemůže být stejný jako zdrojový sklad
        """;
    public const string StoreTransactionReasonAndSourceStoreInvalidMessage = """
        ID cílového skladu musí být nastaveno jen přesně když je důvod transakce přesun
        """;
    public const string StoreTransactionItemsNotUniqueMessage = """
        Skladová transakce obsahuje duplicitní skladové položky
        """;
    public const string StoreTransactionItemsContainContainerItemsMessage = """
        Nemožno vytvořit skladovou transakci pro kegové položky
        """;

    // strings
    public const string NameTooLongMessage = $"""
        {NamePropName} {TooLongMessage}
        """;
    public const string NameEmptyMessage = $"""
        {NamePropName} nemůže být prázdné
        """;
    public const string UnitNameTooLongMessage = $"""
        {UnitNamePropName} {TooLongMessage}
        """;
    public const string UnitNameEmptyMessage = $"""
        {UnitNamePropName} nemůže být prázdná
        """;
    public const string NoteTooLongMessage = $"""
        {NotePropName} {TooLongMessage}
        """;
    public const string NoteEmptyMessage = $"""
        {NotePropName} nemůže být prázdná
        """;
    public const string DescriptionTooLongMessage = $"""
        {DescriptionPropName} {TooLongMessage}
        """;

    // IDs
    public const string StoreIdNotValidMessage = $"""
        {StoreIdPropName} neodpovídá žádnému existujícímu skladu
        """;
    public const string StoreItemIdNotValidMessage = $"""
        {StoreItemIdPropName} neodpovídá žádné existující skladové položce
        """;
    public const string CompositeIdNotValidMessage = $"""
        {CompositeIdPropName} neodpovídá žádné existující složené položce
        """;
    public const string ContainerIdNotValidMessage = $"""
        {ContainerIdPropName} neodpovídá žádnému existujícímu kegu
        """;
    public const string ContainerItemIdNotValidMessage = $"""
        {StoreItemIdPropName} neodpovídá žádné existující kegové položce
        """;
    public const string ContainerTemplateIdNotValidMessage = $"""
        {ContainerTemplateIdPropName} neodpovídá žádnému existujícímu vzoru kegu
        """;
    public const string PipeIdNotValidMessage = $"""
        {PipeIdPropName} neodpovídá žadné existující pípě
        """;
    public const string CategoryIdNotValidMessage = $"""
        {CategoryIdPropName} neodpovídá žadné existující kategorii
        """;
    public const string CashBoxIdNotValidMessage = $"""
        {CashBoxIdPropName} neodpovídá žádné existující kase
        """;
    public const string CategoryIdsNotValidMessage = $"""
        Aspoň jedno z {CategoryIdsPropName} neodpovídá žadné existující kategorii
        """;
    public const string TargetIdsNotValidMessage = $"""
        Aspoň jedno z {TargetIdsPropName} neodpovídá žádné existující prodejní položce
        """;
    public const string ModifierIdsNotValidMessage = $"""
        Aspoň jedno z {ModifierIdsPropName} neodpovídá žádnému existujícímu modifikátoru
        """;
    public const string StoreItemIdsNotValidMessage = $"""
        Aspoň jedno z {StoreItemIdPropName} neodpovídá žádné existující skladové položce
        """;
}
