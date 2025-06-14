namespace KisV4.BL.EF.Services;

public static class DictionaryExtensions {
    public static void AddItemOrCreate<TKey, TValue>(
        this Dictionary<TKey, TValue[]> dict,
        TKey key,
        TValue value
    ) where TKey : notnull {
        dict[key] = dict.TryGetValue(key, out var arr) ? [.. arr, value] : [value];
    }
}
