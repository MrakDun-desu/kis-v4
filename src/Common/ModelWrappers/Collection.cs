namespace KisV4.Common.ModelWrappers;

public abstract record CollectionResponse<T> {
    public required T[] Data { get; set; }
}
