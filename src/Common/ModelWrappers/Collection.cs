namespace KisV4.Common.ModelWrappers;

public abstract record CollectionResponse<T> {
    public required IEnumerable<T> Data { get; set; }
}
