using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;
using Isu.Services;
using Xunit;
using Xunit.Sdk;

namespace Isu.Test;

public class IsuServiceTest
{
    private IsuService _isu = new IsuService();

    [Fact]
    public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
    {
        var courseNumber = new CourseNumber(2);
        var groupName = new GroupName("M32041");
        Group<Student> group = _isu.AddGroup(groupName, courseNumber);

        Student student = _isu.AddStudent(group, "Me");

        Assert.Equal(group, student.Group);
        Assert.Equal(student, group.GetStudent(student.Id));
    }

    [Fact]
    public void ReachMaxStudentPerGroup_ThrowException()
    {
        var courseNumber = new CourseNumber(2);
        var groupName = new GroupName("M33041");
        Group<Student> group = _isu.AddGroup(groupName, courseNumber);

        for (int i = 0; i < Group<Student>.MaxStudentAmount; i++)
            _isu.AddStudent(group, "Me");

        Assert.Throws<IsuException>(() => _isu.AddStudent(group, "Me"));
    }

    [Fact]
    public void CreateGroupWithInvalidName_ThrowException()
    {
        Assert.Throws<IsuException>(() => new GroupName("K320411"));
    }

    [Fact]
    public void TransferStudentToAnotherGroup_GroupChanged()
    {
        var courseNumber = new CourseNumber(2);
        var firstGroupName = new GroupName("M32041");
        var secondGroupName = new GroupName("M32001");

        Group<Student> firstGroup = _isu.AddGroup(firstGroupName, courseNumber);
        Group<Student> secondGroup = _isu.AddGroup(secondGroupName, courseNumber);

        Student student = _isu.AddStudent(firstGroup, "Me");

        _isu.ChangeStudentGroup(student, secondGroup);

        Assert.Equal(secondGroup, student.Group);
        Assert.Null(firstGroup.FindStudent(student.Id));
        Assert.Equal(student, secondGroup.GetStudent(student.Id));
    }
}