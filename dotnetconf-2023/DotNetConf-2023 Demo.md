# Setup

Open browser links:

* https://learn.microsoft.com/en-us/dotnet/csharp/
* https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md
* https://github.com/dotnet/csharplang

# Intro

Share with the audience how they can engage with the C# language. The browser links represent an increasing
level of engagement.

# Demos

## Using Type Aliases

As a C# 12 warm-up, let's talk about using type aliases. These have been in the language since C# 1.0 and have
always required a fully-qualified type name on the right-hand side.

```C#
using Grade = System.Decimal;
```

In C# 12, the using alias type syntax has been expanded to allow many other type name syntax forms. For
example, you can use a predefined type keyword. _Change `System.Decimal` to `decimal`._

```C#
using Grade = decimal;
```

In addition, you could be more sophisticated and use tuple syntax. _Change `decimal` to `(string, decimal)`._

```C#
using Grade = (string, decimal);
```

Of course, you could always create using type aliases for tuples by referencing `System.ValueTuple<string, decimal`>.
However, you could never add tuple names, but that's easy now. _Add the names, `"Course"` and `"Value"`._

```C#
using Grade = (string Course, decimal Value);
```

Prior to C# 12, it hasn't been possible to create using type aliases for pointer types. _Add a `*` after the
tuple._

```C#
using Grade = (string Course, decimal Value)*;
```

Hover over the error squiggle on the pointer type. It says that pointers are only allowed in unsafe contexts.
How can we fix that? By adding the unsafe keyword to your using type alias. _Add `unsafe` after `using`._

```C#
using unsafe Grade = (string Course, decimal Value)*;
```

## Primary Constructors

Here is our starting code:

```C#
using Grade = System.Decimal;

var mads = new Student("Mads Torgersen", 900751, new[] { 3.5m, 2.9m, 1.8m });

WriteLine(mads);

public record class Student(string Name, int Id, Grade[] Grades)
{
    public Student(string name, int id) : this(name, id, Array.Empty<Grade>()) { }

    public Grade GPA => Grades switch
    {
        [] => 4.0m,
        [var grade] => grade,
        [.. var all] => all.Average()
    };
}
```

We're starting with a positional record, which has primary constructor that declares the init-only properties
of the record. _In `WriteLine(mads)`, type `.` after `mads` to show the properties in the completion list.
Go ahead and select `Name`._

```C#
WriteLine(mads.Name);
```

In C# 12, we've added support for primary constructors on non-record classes and structs. _Remove the
`record` keyword._

```C#
public class Student(string Name, int Id, Grade[] Grades)
```

Note that the primary constructor no longer declares properties. Instead, it declares parameters, just like
a regular constructor. That's why there's an error on `mads.Name` and warnings on a couple of the parameters
because they're unread. _Change `mads.Name` to `mads.GPA` to get rid of the error and show that the other
properties are no longer available in the completion list. Then, use Rename to update the parameter names to
be lowercase._

```C#
WriteLine(mads.GPA);

public class Student(string name, int id, Grade[] grades)
```

Primary constructors provide a brief and simple way to declare the main constructor for a class, but what
if it inherits from another class and we need to pass parameters into a base constructo? This is easily
done! _Add `: Person(name)` after the primary constructor:

```C#
public class Student(string name, int id, Grade[] grades) : Person(name)
```

The `name` and `id` parameters are still unread, so let's address that. We can use the `name` parameter to
initialize a public auto-property. _Add `public string Name { get; set; } = name;`._

```C#
public class Student(string name, int id, Grade[] grades) : Person(name)
{
    public Student(string name, int id) : this(name, id, Array.Empty<Grade>()) { }

    public string Name { get; set; } = name;
```

We can use 'id' and return it from a getter-only auto-propty. _Add `public int Id => id;`._

```C#
public class Student(string name, int id, Grade[] grades) : Person(name)
{
    public Student(string name, int id) : this(name, id, Array.Empty<Grade>()) { }

    public string Name { get; set; } = name;
    public int Id => id;
```

Now the warnings are all gone!

So, can we use the `name` parameter to do other things? _Try to changethe expression body of `Id` to
`name.Length`._ Note that the completion list doesn't actually contain `name` and auto-completes to `Name`. Why is that?

```C#
public class Student(string name, int id, Grade[] grades) : Person(name)
{
    public Student(string name, int id) : this(name, id, Array.Empty<Grade>()) { }

    public string Name { get; set; } = name;
    public int Id => Name.Length;
```

The reason that `name` doesn't appear in the list is because the C# language service is trying to avoid
a "double capture". This happens when you deliberately capture a primary constructor parameter value in a
field or property, and then use the parameter again elsewhere and the compiler automatically captures it. This
can result in the same field being stored in two separate fields, which is probably unintended.

**Refactor to regular constructor with fields and refactor back to primary constructor with and without fields**

**Constructor Chaining**

## Collection Expressions

Raw Notes:

- Change “grades” parameter to a `List<Grade>` and show that they don’t work anymore
    - Now we have to fix up the call sites.
- Change “grades” to an `ImmutableList<Grade>` and the errors are completely different.
- Change “grades” to an `ImmutableArray<Grade>` and the errors go away but it’s awful and is going to fail at runtime! Try to run it and show how it fails.
- Change the first callsite to a collection expression manually
- Change the second callsite to a collection expression using the code fix.
- Talk about syntax correspondence with list patterns
- Talk about spreads by bringing in `Grades.Dustin`.
- Now, how is the sausage made.
- F12 on `ImmutableArray<T>` to show the `CollectionBuilder` attribute.
- F12 on `ImmutableArray` to show the `Create` method that it will call.
- Cycle through different types, ImmutableList, List, array, and finally interfaces, such as `IList<T>`.
- There are a couple of things they don’t yet do.
    - There’s no “natural type” so that don’t work with var.
    - Dictionary expressions