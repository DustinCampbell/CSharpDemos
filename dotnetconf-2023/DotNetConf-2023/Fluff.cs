global using System.Collections.Immutable;
global using static System.Console;

public class Person(string name)
{
}

public static class Grades
{
    private static readonly decimal[] s_dustin = { 3.9m, 3.75m, 4.0m };

    public static ReadOnlySpan<decimal> Dustin => s_dustin;
}