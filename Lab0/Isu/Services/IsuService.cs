using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;

namespace Isu.Services;

public class IsuService : IIsuService
{
    private readonly List<Group<Student>> _groupsList;
    private int _studentId;
    public IsuService(List<Group<Student>> groupsList)
    {
        _studentId = 0;
        _groupsList = groupsList;
    }

    public IsuService()
    {
        _studentId = 0;
        _groupsList = new List<Group<Student>>();
    }

    public Group<Student> AddGroup(GroupName name, CourseNumber courseNumber)
    {
        if (courseNumber is null)
            throw new ArgumentException("Course number cannot be null");
        if (name is null)
            throw new ArgumentException("Group name cannot be null");

        var group = new Group<Student>(courseNumber, name, new List<Student>());
        _groupsList.Add(group);
        return group;
    }

    public Group<Student> AddGroup(GroupName name, CourseNumber courseNumber, List<Student> studentsList)
    {
        if (studentsList.Count > Group<Student>.MaxStudentAmount)
            throw new IsuException("Group overflow");
        if (courseNumber is null)
            throw new ArgumentException("Course number cannot be null");
        if (name is null)
            throw new ArgumentException("Group name cannot be null");

        var group = new Group<Student>(courseNumber, name, studentsList);
        _groupsList.Add(group);
        return group;
    }

    public Student AddStudent(Group<Student> group, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Students name is empty");
        if (group is null)
            throw new ArgumentException("Group cannot be null");

        var student = new Student(++_studentId, name, group);
        group.AddStudent(student);
        return student;
    }

    public Student GetStudent(int id)
    {
        Student? student = FindStudent(id);

        if (student is null)
            throw new ArgumentException("Invalid id");

        return student;
    }

    public Student? FindStudent(int id)
    {
        return _groupsList.Find(x => x.FindStudent(id) is not null)?.FindStudent(id);
    }

    public IReadOnlyList<Student>? FindStudents(GroupName groupName)
    {
        if (groupName is null)
            throw new ArgumentException("Group name cannot be null");

        Group<Student>? group = _groupsList.Find(x => x.GroupName.Equals(groupName));
        return group?.StudentsList;
    }

    public IReadOnlyList<Student>? FindStudents(CourseNumber courseNumber)
    {
        if (courseNumber is null)
            throw new ArgumentException("Course number cannot be null");

        IEnumerable<Student> students
            = _groupsList
            .Where(x => x.CourseNumber.Equals(courseNumber))
            .SelectMany(g => g.StudentsList);

        return students.ToList();
    }

    public Group<Student> GetGroup(GroupName groupName)
    {
        if (groupName is null)
            throw new ArgumentException("Group name cannot be null");

        Group<Student>? group = FindGroup(groupName);
        if (group is null)
            throw new ArgumentException("Invalid name");
        return group;
    }

    public Group<Student>? FindGroup(GroupName groupName)
    {
        if (groupName is null)
            throw new ArgumentException("Group name cannot be null");

        return _groupsList.Find(x => x.GroupName.Equals(groupName));
    }

    public IReadOnlyList<Group<Student>> FindGroups(CourseNumber courseNumber)
    {
        if (courseNumber is null)
            throw new ArgumentException("Course number cannot be null");

        return _groupsList.FindAll(x => x.CourseNumber.Equals(courseNumber));
    }

    public void ChangeStudentGroup(Student student, Group<Student> newGroup)
    {
        if (student is null)
            throw new ArgumentException("Student cannot be null");
        if (newGroup is null)
            throw new ArgumentException("Group cannot be null");
        student.ChangeGroup(newGroup);
    }
}