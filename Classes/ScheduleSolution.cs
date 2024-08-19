namespace ConsoleApp1.Classes
{
    public class ScheduleSolution
    {
        List<List<List<string>>> Calendar = new List<List<List<string>>>();

        ScheduleSolution()
        {
            for (int day = 0; day < Schedule.GetDaysWithSessions(); day++)
            {
                for (int slot = 0; slot < Schedule.GetSlotsInDay(day); slot++)
                {
                    foreach (Session session in Schedule.GetCalendar()[day][slot])
                    {
                        Calendar[day][slot].Add(session.GetName());
                    }
                }
            }
        }
    }
}
