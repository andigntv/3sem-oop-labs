using Banks.Exceptions;

namespace Banks.Models;

public class CustomDateTime
{
    static CustomDateTime()
    {
        Start = DateTime.Now;
        Now = Start;
    }

    public static DateTime Start { get; }
    public static DateTime Now { get; private set; }

    public static void SetNow(DateTime dateTime) // CustomDateTime has public set because it is just model for lab, irl I won't use this
    {
        if (dateTime.CompareTo(CustomDateTime.Now) < 0)
            throw new TimeException("You can't go back in time(");
        Now = dateTime;
    }
}