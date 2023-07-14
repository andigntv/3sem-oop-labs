using Isu.Exceptions;

namespace Isu.Models;

public class CourseNumber
{
    private const int MaxCourseNumber = 6;
    private const int MinCourseNumber = 1;

    public CourseNumber(int number)
    {
        if (number is < MinCourseNumber or > MaxCourseNumber)
            throw new IsuException("Invalid course number");

        Number = number;
    }

    public int Number { get; }
}