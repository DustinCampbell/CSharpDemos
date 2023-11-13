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