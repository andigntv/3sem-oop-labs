using System.Text.RegularExpressions;
using Isu.Extra.Exceptions;

namespace Isu.Extra.Entities;

public class Teacher
{
    public Teacher(string secondName, string firstName, string patronymic = "")
    {
        if (string.IsNullOrWhiteSpace(secondName))
            throw new ArgumentException("Second name cannot be empty");
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty");

        if (secondName.All(char.IsLetter) is false)
            throw new NameException("Invalid second name");
        if (firstName.All(char.IsLetter) is false)
            throw new NameException("Invalid first name");
        if (patronymic.All(char.IsLetter) is false)
            throw new NameException("Invalid patronymic");

        FirstName = firstName;
        SecondName = secondName;
        Patronymic = patronymic;
    }

    public string FirstName { get; }
    public string SecondName { get; }
    public string Patronymic { get; }
}