using System.Runtime.CompilerServices;

namespace P3Task.DocumentManagement.Core.Helpers;

public static class NullableHelper
{
    public delegate bool TryDelegate<T>(string s, out T result);

    public static bool TryParseNullable<T>(string? s, out T? result, TryDelegate<T> tryDelegate) where T : struct
    {
        if (s == null)
        {
            result = null;
            return true;
        }

        var success = tryDelegate(s, out var temp);
        result = temp;
        return success;
    }
}