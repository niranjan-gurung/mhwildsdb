namespace mhwildsdb.Helpers;

public static class ValidationHelpers
{
    public static bool BeValidName(string name) =>
        name!.All(c => char.IsLetter(c) || c == ' ' || c == '\'');

    public static bool BeUnique<T, TKey>(
        ICollection<T> collection,
        Func<T, TKey> selector)
    {
        return collection.Select(selector)
                .Distinct()
                .Count() == collection.Count;
    }
}
