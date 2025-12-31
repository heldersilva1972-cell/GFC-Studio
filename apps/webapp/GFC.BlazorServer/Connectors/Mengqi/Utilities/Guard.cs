using System;

namespace GFC.BlazorServer.Connectors.Mengqi.Utilities;

internal static class Guard
{
    public static void AgainstOutOfRange(int value, int min, int max, string paramName)
    {
        if (value < min || value > max)
        {
            throw new ArgumentOutOfRangeException(paramName, $"Value {value} must be between {min} and {max}.");
        }
    }

    public static T NotNull<T>(T? value, string paramName) where T : class
    {
        if (value is null)
        {
            throw new ArgumentNullException(paramName);
        }

        return value;
    }
}


