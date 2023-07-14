using Isu.Extra.Exceptions;

namespace Isu.Extra.Entities;

public class ElectiveGroup : IEquatable<ElectiveGroup>
{
    public const int MaxStudentAmount = 30;
    private List<NewStudent> _students;

    public ElectiveGroup(string name, Faculty faculty)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");

        Name = name;
        Faculty = faculty;
        _students = new List<NewStudent>();
        Timetable = new Timetable();
    }

    public Faculty Faculty { get; }
    public string Name { get; }
    public IReadOnlyList<NewStudent> Students => _students;
    public Timetable Timetable { get; private set; }

    public void AddLesson(Lesson lesson)
    {
        Timetable.AddLesson(lesson);
    }

    public void AddStudent(NewStudent student)
    {
        if (student is null)
            throw new ArgumentException("Student cannot be null");
        if (_students.Count is MaxStudentAmount)
            throw new GroupException("Group is already full");
        _students.Add(student);
    }

    public void RemoveStudent(NewStudent student)
    {
        if (_students.Remove(student) is false)
            throw new GroupException("Cannot remove student");
    }

    public bool Equals(ElectiveGroup? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Faculty == other.Faculty && Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ElectiveGroup)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Faculty, Name);
    }
}