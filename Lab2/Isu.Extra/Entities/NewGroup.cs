using Isu.Entities;
using Isu.Extra.Exceptions;
using Isu.Models;

namespace Isu.Extra.Entities;

public class NewGroup : Group<NewStudent>
{
    public NewGroup(CourseNumber courseNumber, GroupName groupName)
        : base(courseNumber, groupName)
    {
        Timetable = new Timetable();
        Faculty = groupName.Identifier[0] switch
        {
            'M' => Faculty.Tint,
            'K' => Faculty.Ktu,
            'B' => Faculty.Btins,
            'F' => Faculty.Fotonica,
            _ => throw new NameException("There is no such faculty")
        };
    }

    public Faculty Faculty { get; }
    public Timetable Timetable { get; private set; }
    public bool CheckCollusion(ElectiveGroup electiveGroup)
    {
        return Timetable.CheckCollusion(electiveGroup.Timetable);
    }

    public void AddLesson(Lesson lesson)
    {
        Timetable.AddLesson(lesson);
    }
}