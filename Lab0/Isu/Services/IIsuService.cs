using Isu.Entities;
using Isu.Models;

namespace Isu.Services;

public interface IIsuService
{
    Group<Student> AddGroup(GroupName name, CourseNumber courseNumber);
    Group<Student> AddGroup(GroupName name, CourseNumber courseNumber, List<Student> studentsList);
    Student AddStudent(Group<Student> group, string name);

    Student GetStudent(int id);
    Student? FindStudent(int id);
    IReadOnlyList<Student>? FindStudents(GroupName groupName);
    IReadOnlyList<Student>? FindStudents(CourseNumber courseNumber);

    Group<Student>? FindGroup(GroupName groupName);
    IReadOnlyList<Group<Student>>? FindGroups(CourseNumber courseNumber);

    void ChangeStudentGroup(Student student, Group<Student> newGroup);
}