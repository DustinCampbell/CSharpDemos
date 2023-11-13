global using System.Collections.Immutable;
global using static System.Console;

#pragma warning disable CS9113 // Parameter is unread.
public class Person(string name)
#pragma warning restore CS9113 // Parameter is unread.
{
}

public static class Grades
{
    private static readonly decimal[] s_dustin = { 3.9m, 3.75m, 4.0m };

    public static ReadOnlySpan<decimal> Dustin => s_dustin;
}