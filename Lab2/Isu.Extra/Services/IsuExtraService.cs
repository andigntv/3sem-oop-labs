using Isu.Entities;
using Isu.Exceptions;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Models;

namespace Isu.Extra.Services;

public class IsuExtraService
{
    private List<NewGroup> _groups;
    private List<Elective> _electives;
    private int _studentId = 0;

    public IsuExtraService()
    {
        _groups = new List<NewGroup>();
        _electives = new List<Elective>();
    }

    public IReadOnlyList<NewStudent> StudentsWithoutElectives()
    {
        var students = (from @group in _groups from student in @group.StudentsList where student.FirstElective is null select student).ToList();
        return students;
    }

    public NewGroup AddGroup(GroupName name, CourseNumber courseNumber)
    {
        if (courseNumber is null)
            throw new ArgumentException("Course number cannot be null");
        if (name is null)
            throw new ArgumentException("Group name cannot be null");

        var group = new NewGroup(courseNumber, name);
        _groups.Add(group);
        return group;
    }

    public NewGroup AddGroup(GroupName name, CourseNumber courseNumber, List<NewStudent> studentsList)
    {
        if (studentsList.Count > Group<Student>.MaxStudentAmount)
            throw new IsuException("Group overflow");
        if (courseNumber is null)
            throw new ArgumentException("Course number cannot be null");
        if (name is null)
            throw new ArgumentException("Group name cannot be null");

        var group = new NewGroup(courseNumber, name);
        _groups.Add(group);
        return group;
    }

    public NewStudent AddStudent(NewGroup group, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("NewStudents name is empty");
        if (group is null)
            throw new ArgumentException("Group cannot be null");

        var student = new NewStudent(++_studentId, name, group);
        group.AddStudent(student);
        return student;
    }

    public NewStudent GetStudent(int id)
    {
        NewStudent? student = FindStudent(id);

        if (student is null)
            throw new ArgumentException("Invalid id");

        return student;
    }

    public NewStudent? FindStudent(int id)
    {
        return _groups.Find(x => x.FindStudent(id) is not null)?.FindStudent(id);
    }

    public IReadOnlyList<NewStudent>? FindStudents(GroupName groupName)
    {
        if (groupName is null)
            throw new ArgumentException("Group name cannot be null");

        NewGroup? group = _groups.Find(x => x.GroupName.Equals(groupName));
        return group?.StudentsList;
    }

    public IReadOnlyList<NewStudent>? FindStudents(CourseNumber courseNumber)
    {
        if (courseNumber is null)
            throw new ArgumentException("Course number cannot be null");

        IEnumerable<NewStudent> students
            = _groups
            .Where(x => x.CourseNumber.Equals(courseNumber))
            .SelectMany(g => g.StudentsList);

        return students.ToList();
    }

    public NewGroup GetGroup(GroupName groupName)
    {
        if (groupName is null)
            throw new ArgumentException("Group name cannot be null");

        NewGroup? group = FindGroup(groupName);
        if (group is null)
            throw new ArgumentException("Invalid name");
        return group;
    }

    public NewGroup? FindGroup(GroupName groupName)
    {
        if (groupName is null)
            throw new ArgumentException("Group name cannot be null");

        return _groups.Find(x => x.GroupName.Equals(groupName));
    }

    public IReadOnlyList<NewGroup> FindGroups(CourseNumber courseNumber)
    {
        if (courseNumber is null)
            throw new ArgumentException("Course number cannot be null");

        return _groups.FindAll(x => x.CourseNumber.Equals(courseNumber));
    }

    public void ChangeStudentGroup(NewStudent student, NewGroup group)
    {
        if (student is null)
            throw new ArgumentException("NewStudent cannot be null");
        if (group is null)
            throw new ArgumentException("Group cannot be null");
        student.ChangeGroup(group);
    }

    public void RemoveFirstElective(NewStudent student)
    {
        if (student is null)
            throw new ArgumentException("Student cannot be null");
        student.RemoveFirstElective();
    }

    public void RemoveSecondElective(NewStudent student)
    {
        if (student is null)
            throw new ArgumentException("Student cannot be null");
        student.RemoveSecondElective();
    }

    public Elective AddElective(string name, Faculty faculty)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name of elective cannot be null");
        var elective = new Elective(name, faculty);
        _electives.Add(elective);
        return elective;
    }

    public ElectiveGroup AddGroupInElective(string electiveName, string groupName, Faculty faculty)
    {
        if (string.IsNullOrWhiteSpace(electiveName))
            throw new ArgumentException("Name of elective cannot be null");
        if (string.IsNullOrWhiteSpace(groupName))
            throw new ArgumentException("Name of group cannot be null");

        Elective? elective = _electives.Find(x => x.Name.Equals(electiveName) && x.Faculty.Equals(faculty));
        if (elective is null)
            throw new ElectiveException("There is no such an elective");
        return elective.AddGroup(electiveName);
    }

    public IReadOnlyList<ElectiveGroup> GetGroups(Elective elective)
    {
        if (elective is null)
            throw new ArgumentException("Elective cannot be null");
        return elective.Groups;
    }

    public IReadOnlyList<NewStudent> GetStudnets(ElectiveGroup electiveGroup)
    {
        if (electiveGroup is null)
            throw new ArgumentException("Elective group cannot be null");
        return electiveGroup.Students;
    }

    public void AddLesson(NewGroup group, Lesson lesson)
    {
        if (group is null)
            throw new ArgumentException("Group cannot be null");
        if (lesson is null)
            throw new ArgumentException("Lesson cannot be null");
        NewGroup? newGroup = _groups.Find(x => x.GroupName.Equals(group.GroupName));
        if (newGroup is null)
            throw new ArgumentException("There is no such a group");
        newGroup.AddLesson(lesson);
    }

    public void AddLesson(ElectiveGroup group, Lesson lesson)
    {
        if (group is null)
            throw new ArgumentException("Group cannot be null");
        if (lesson is null)
            throw new ArgumentException("Lesson cannot be null");

        ElectiveGroup? electiveGroup = _electives.Where(elective => elective.Faculty.Equals(group.Faculty)).Select(elective => elective.FindGroup(group)).FirstOrDefault();
        if (electiveGroup is null)
            throw new ArgumentException("There is no such a group");
        electiveGroup.AddLesson(lesson);
    }

    public void PutStudentInElectiveGroup(NewStudent student, ElectiveGroup group)
    {
        if (student is null)
            throw new ArgumentException("Student cannot be null");
        if (group is null)
            throw new ArgumentException("Group cannot be null");
        student.AddElective(group);
        group.AddStudent(student);
    }
}