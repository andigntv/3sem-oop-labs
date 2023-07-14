using Isu.Exceptions;

namespace Isu.Entities;

public class Student
{
    private const int MinId = 0;
    public Student(int id, string name, Group<Student> group)
    {
        if (id < MinId)
            throw new ArgumentException("Id cannot be");
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Students name is empty");
        if (group is null)
            throw new ArgumentException("Group cannot be null");
        Id = id;
        Name = name;
        Group = group;
    }

    public int Id { get; }
    public string Name { get; }
    public Group<Student> Group { get; private set; }

    public void ChangeGroup(Group<Student> newGroup)
    {
        newGroup.AddStudent(this);
        Group.RemoveStudent(this);
        Group = newGroup;
    }
}