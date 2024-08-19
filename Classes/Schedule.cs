namespace ConsoleApp1.Classes
{
    public static class Schedule
    {
        // Days; Slots; Sessions in slot
        static List<List<List<Session>>> Calendar = new List<List<List<Session>>>(); // Point 1

        // Point 2
        public static bool CheckIfSessionOnDay(Session session, int day)
        {
            List<List<Session>> SessionsForDay = Calendar[day];

            // Checking to see if session is part of a session group. If it is, see if that session
            // group already has a session on that day.
            if (!session.GetSessionGroup().Equals(""))
            {
                for (int i = 0; i < SessionsForDay.Count; i++)
                {
                    List<Session> SessionSlot = SessionsForDay[i];
                    foreach (Session session1 in SessionSlot)
                    {
                        if (session1.GetSessionGroup().Equals(session.GetSessionGroup()))
                            return true;
                    }
                }
            }

            for (int i = 0; i < SessionsForDay.Count; i++)
            {
                List<Session> SessionSlot = SessionsForDay[i];
                if (SessionSlot.Contains(session))
                    return true;
            }
            return false;
        }

        public static bool GetIfStudentOrInstructorHasSessionAtSlot(Session currentSession, int day, int slot)
        {
            List<Instructor> instructor = currentSession.GetInstructors();
            List<Student> students = currentSession.GetStudents();
            foreach (Session session in Calendar[day][slot])
            {
                if (session.GetInstructors() == instructor)
                    return true;
                foreach (Student student in students)
                {
                    if (session.GetStudents().Contains(student))
                        return true;
                }
            }
            return false;
        }

        // See if next slot or previous slot contains a session that can't go after or before this session
        public static bool CheckIfSessionCanBeAfterOrBeforeSpecificSession(Session session, int day, int slot)
        {
            if (slot < Calendar[day].Count - 1)
            {
                if (Calendar[day][slot + 1].Contains(session.GetSessionNotAfter()))
                    return false;
            }

            if (slot > 0)
            {
                if (Calendar[day][slot - 1].Contains(session.GetSessionNotBefore()))
                    return false;
            }
            return true;
        }


        // Point 3
        public static int GetDaysWithSessions()
        {
            return Calendar.Count;
        }

        // Point 4
        public static int GetSlotsInDay(int Day)
        {
            return Calendar[Day].Count;
        }

        public static void AddSessionsToSlots(int day, int slot, Session session)
        {
            int LengthOfSession = session.GetLengthOfSessions();
            for (int i = 0; i < LengthOfSession; i++)
            {
                Calendar[day][slot + i].Add(session);
            }
        }

        public static void RemoveSessionsFromSlots(Session session, int day, int slot)
        {
            int LengthOfSession = session.GetLengthOfSessions();
            for (int i = 0; i < LengthOfSession; i++)
            {
                Calendar[day][slot].Remove(session);
            }
        }

        public static List<List<List<Session>>> GetCalendar()
        {
            return Calendar;
        }
    }
}
