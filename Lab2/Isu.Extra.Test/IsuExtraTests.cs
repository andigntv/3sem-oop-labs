using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Extra.Services;
using Isu.Models;
using Xunit;

namespace Isu.Extra.Test;

public class IsuExtraTests
{
    private IsuExtraService _isu = new IsuExtraService();

    [Fact]
    public void GroupLessonsAndElectiveLessonsCollusion()
    {
        var groupLesson = new Lesson(Week.Monday, "15:20", "16:50", new Teacher("Lalal", "Alal"), 301);
        var electiveLesson = new Lesson(Week.Monday, "14:00", "15:30", new Teacher("Sname", "Fname"), 1243);

        NewGroup group = _isu.AddGroup(new GroupName("M32041"), new CourseNumber(2));
        NewStudent student = _isu.AddStudent(group, "Me");

        Elective elective = _isu.AddElective("Kiberbez", Faculty.Ktu);
        ElectiveGroup electiveGroup = _isu.AddGroupInElective("Kiberbez", "P2231", Faculty.Ktu);

        _isu.AddLesson(group, groupLesson);
        _isu.AddLesson(electiveGroup, electiveLesson);

        Assert.Throws<TimeException>(() => student.AddElective(electiveGroup));
    }

    [Fact]
    public void TryingToChooseElectivesFromOwnFaculty()
    {
        NewGroup group = _isu.AddGroup(new GroupName("M32041"), new CourseNumber(2));
        NewStudent student = _isu.AddStudent(group, "Me");

        Elective elective = _isu.AddElective("Kiberbez", Faculty.Tint);
        ElectiveGroup electiveGroup = _isu.AddGroupInElective("Kiberbez", "P2231", Faculty.Tint);

        Assert.Throws<ElectiveException>(() => student.AddElective(electiveGroup));
    }

    [Fact]
    public void AddElective()
    {
        NewGroup group = _isu.AddGroup(new GroupName("M32041"), new CourseNumber(2));
        NewStudent student = _isu.AddStudent(group, "Me");

        Elective firstElective = _isu.AddElective("Kiberbez", Faculty.Ktu);
        ElectiveGroup firstElectiveGroup = _isu.AddGroupInElective("Kiberbez", "P2231", Faculty.Ktu);

        Elective secondElective = _isu.AddElective("Refrigerators", Faculty.Btins);
        ElectiveGroup secondElectiveGroup = _isu.AddGroupInElective("Refrigerators", "H3127", Faculty.Btins);

        _isu.PutStudentInElectiveGroup(student, firstElectiveGroup);
        _isu.PutStudentInElectiveGroup(student, secondElectiveGroup);

        Assert.Equal(firstElectiveGroup, student.FirstElective);
        Assert.Equal(secondElectiveGroup, student.SecondElective);
    }

    [Fact]
    public void RemoveElective()
    {
        NewGroup group = _isu.AddGroup(new GroupName("M32041"), new CourseNumber(2));
        NewStudent student = _isu.AddStudent(group, "Me");

        Elective firstElective = _isu.AddElective("Kiberbez", Faculty.Ktu);
        ElectiveGroup firstElectiveGroup = _isu.AddGroupInElective("Kiberbez", "P2231", Faculty.Ktu);

        Elective secondElective = _isu.AddElective("Refrigerators", Faculty.Btins);
        ElectiveGroup secondElectiveGroup = _isu.AddGroupInElective("Refrigerators", "H3127", Faculty.Btins);

        _isu.PutStudentInElectiveGroup(student, firstElectiveGroup);
        _isu.PutStudentInElectiveGroup(student, secondElectiveGroup);

        _isu.RemoveFirstElective(student);
        Assert.Equal(secondElectiveGroup, student.FirstElective);
        Assert.Null(student.SecondElective);
    }
}