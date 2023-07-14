using Isu.Exceptions;
using Isu.Models;

namespace Isu.Entities;

public class Group<TStudent>
    where TStudent : Student
{
    public const int MaxStudentAmount = 30;
    private readonly List<TStudent> _studentsList;
    public Group(CourseNumber courseNumber, GroupName groupName, List<TStudent> studentsList)
        : this(courseNumber, groupName)
    {
        if (studentsList.Count > MaxStudentAmount)
            throw new IsuException("Group overflow");
        _studentsList = studentsList;
    }

    public Group(CourseNumber courseNumber, GroupName groupName)
    {
        if (courseNumber is null)
            throw new ArgumentException("Course number cannot be null");
        if (groupName is null)
            throw new ArgumentException("Group name cannot be null");
        CourseNumber = courseNumber;
        GroupName = groupName;
        _studentsList = new List<TStudent>();
    }

    public IReadOnlyList<TStudent> StudentsList => _studentsList;
    public CourseNumber CourseNumber { get; }
    public GroupName GroupName { get; }

    public TStudent? FindStudent(int id)
    {
        return _studentsList.Find(x => x.Id == id);
    }

    public TStudent GetStudent(int id)
    {
        TStudent? student = FindStudent(id);
        if (student is null)
            throw new ArgumentException("Invalid id");
        return student;
    }

    public void RemoveStudent(TStudent student)
    {
        if (_studentsList.Remove(student) is false)
            throw new IsuException("Cannot remove student");
    }

    public void RemoveStudent(int id)
    {
        TStudent student = GetStudent(id);
        if (_studentsList.Remove(student))
            throw new IsuException("Cannot remove student");
    }

    public void AddStudent(TStudent student)
    {
        if (_studentsList.Count is MaxStudentAmount)
            throw new IsuException("Group is already full");
        _studentsList.Add(student);
    }
}