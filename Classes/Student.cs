namespace ConsoleApp1.Classes
{
    public class Student
    {
        string FirstName; // Point 1
        string LastName; // Point 1
        List<TimeRestriction> TimeRestrictions = new List<TimeRestriction>(); // Point 2

        public Student(List<TimeRestriction> timeRestrictions)
        {
            this.TimeRestrictions = timeRestrictions;
        }
        public bool GetIfCanBeSessionsToday(int day)
        {
            return TimeRestrictions[day].GetIfCanBeSessionsToday();
        }

        public bool GetIfCanBeSessionAtThisSlot(int day, int slot)
        {
            if (!GetIfCanBeSessionsToday(day))
                return false;
            return TimeRestrictions[day].GetIfCanBeSessionAtThisSlot(slot);
        }

        public override bool Equals(object? obj)
        {
            return obj is Student student &&
                   FirstName == student.FirstName &&
                   LastName == student.LastName;
        }
    }
}
