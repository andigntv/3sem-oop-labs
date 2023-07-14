using Isu.Extra.Exceptions;
using Isu.Extra.Models;

namespace Isu.Extra.Entities;

public class Timetable
{
    private List<Lesson> _lessons;

    public Timetable()
    {
        _lessons = new List<Lesson>();
    }

    public void AddLesson(Lesson lesson)
    {
        if (lesson is null)
            throw new ArgumentException("Lesson cannot be null");
        if (CheckCollusion(lesson))
            throw new TimeException("This tame is already taken");
        _lessons.Add(lesson);
    }

    public bool TryAddLesson(Lesson lesson)
    {
        if (lesson is null)
            throw new ArgumentException("Lesson cannot be null");
        if (CheckCollusion(lesson))
            return false;
        _lessons.Add(lesson);
        return true;
    }

    public bool CheckCollusion(Timetable other)
    {
        if (other is null)
            throw new ArgumentException("Timetable cannot be null");
        return _lessons.Any(lesson => other._lessons.Exists(x => x.CheckCollusion(lesson)));
    }

    private bool CheckCollusion(Lesson lesson)
    {
        if (lesson is null)
            throw new ArgumentException("Lesson cannot be null");
        return _lessons.Exists(x => x.CheckCollusion(lesson));
    }
}