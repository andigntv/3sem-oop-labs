using Isu.Extra.Exceptions;
using Isu.Extra.Models;

namespace Isu.Extra.Entities;

public class Lesson : IEquatable<Lesson>
{
    public Lesson(TimeOnly start, TimeOnly end, Teacher teacher, int classroom)
    {
        TimeOnly tempStart = start;
        if (start >= end || (tempStart.AddHours(1.5) != end))
            throw new TimeException("Invalid lesson times");
        Start = start;
        End = end;
        Teacher = teacher;
        Classroom = classroom;
    }

    public Lesson(Week day, string start, string end, Teacher teacher, int classroom)
    {
        var tempStart = TimeOnly.Parse(start);
        Start = TimeOnly.Parse(start);
        End = TimeOnly.Parse(end);
        if (Start >= End || (tempStart.AddHours(1.5) != End))
            throw new TimeException("Invalid lesson times");
        Day = day;
        Teacher = teacher;
        Classroom = classroom;
    }

    public Teacher Teacher { get; }
    public int Classroom { get; }
    public TimeOnly Start { get; }
    public TimeOnly End { get; }
    public Week Day { get; }

    public bool CheckCollusion(Lesson other)
    {
        return (Day.Equals(other.Day) && (other.Start >= Start && other.Start < End))
               || (other.End > Start && other.End <= End);
    }

    public bool Equals(Lesson? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Start.Equals(other.Start) && End.Equals(other.End);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Lesson)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Start, End);
    }
}