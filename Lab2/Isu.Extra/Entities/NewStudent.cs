using Isu.Entities;
using Isu.Extra.Exceptions;
using Isu.Models;

namespace Isu.Extra.Entities;

public class NewStudent : Student
{
    private const int MaxAmountOfElectives = 2;
    private static readonly Group<Student> DefaultGroup = new Group<Student>(new CourseNumber(1), new GroupName("A00000"), new List<Student>());

    // This group is needed so that instead of creating a new group for the base part of each student
    public NewStudent(int id, string name, NewGroup group)
        : base(id, name, DefaultGroup)
    {
        NewGroup = group;
        FirstElective = null;
        SecondElective = null;
    }

    public NewGroup NewGroup { get; private set; }
    public ElectiveGroup? FirstElective { get; private set; }
    public ElectiveGroup? SecondElective { get; private set; }

    public void AddElective(ElectiveGroup elective)
    {
        if (elective is null)
            throw new ArgumentException("Elective cannot be null");
        if (NewGroup.CheckCollusion(elective))
            throw new TimeException("This tame is already taken");
        if (NewGroup.Faculty.Equals(elective.Faculty))
            throw new ElectiveException("Student cannot choose elective from his faculty");
        if (FirstElective is null)
        {
            FirstElective = elective;
            return;
        }

        if (SecondElective is not null) throw new ElectiveException("Two electives are already chosen");
        SecondElective = elective;
    }

    public void RemoveFirstElective()
    {
        FirstElective?.RemoveStudent(this);
        FirstElective = SecondElective;
        SecondElective = null;
    }

    public void RemoveSecondElective()
    {
        SecondElective?.RemoveStudent(this);
        SecondElective = null;
    }

    public void ChangeGroup(NewGroup newGroup)
    {
        newGroup.AddStudent(this);
        NewGroup.RemoveStudent(this);
        NewGroup = newGroup;
    }
}