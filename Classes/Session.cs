namespace ConsoleApp1.Classes
{
    public class Session
    {
        string Name;
        // TODO There is currently no check on if there are any instructors or students in a session.
        // While it is olay for some sessions not to have instructors or not to have students, it
        // can't have none of each. There should be a check for this. Also, there is no check on whether
        // an instructor or student has been added twice. Perhaps turn the List into a Set.
        List<Instructor> Instructors = new List<Instructor>(); // Point 1
        List<Student> Students = new List<Student>(); // Point 1
        int TimesPerWeek; // Point 2
        string SessionGroup = ""; // Point 3
        List<TimeRestriction> TimeRestrictions;// Point 4
        Session? NotAfter; // Point 5
        Session? NotBefore; // Point 5
        List<DayAndSlotNumber> WhenSessionsAre = new List<DayAndSlotNumber>(); // Point 6
        int LengthOfSessions;

        public Session(List<Instructor> instructors, List<Student> students, int timesPerWeek,
            List<TimeRestriction> timeRestrictions, Session? notAfter, Session? notBefore, int lengthOfSessions)
        {
            Instructors = instructors;
            Students = students;
            TimesPerWeek = timesPerWeek;
            TimeRestrictions = timeRestrictions;
            NotAfter = notAfter;
            NotBefore = notBefore;
            LengthOfSessions = lengthOfSessions;
        }

        public string GetName()
        {
            return Name;
        }

        public int GetLengthOfSessions()
        {
            return LengthOfSessions;
        }

        public List<Student> GetStudents()
        {
            return [.. Students];
        }

        public List<Instructor> GetInstructors()
        {
            return [.. Instructors];
        }

        public string GetSessionGroup()
        {
            return SessionGroup;
        }

        public bool GetIfCanBeSessionsToday(int day)
        {
            return TimeRestrictions[day].GetIfCanBeSessionsToday();
        }

        public bool GetIfCanBeSessionAtThisSlot(int day, int slot)
        {
            for (; slot < slot + LengthOfSessions; slot++)
            {
                if (!TimeRestrictions[day].GetIfCanBeSessionAtThisSlot(slot))
                    return false;
            }
            return true;
        }

        public bool GetIfCanBeStudentsSessionToday(int day)
        {
            foreach (Student student in Students)
            {
                if (!student.GetIfCanBeSessionsToday(day))
                {
                    return false;
                }
            }
            return true;
        }

        public bool GetIfCanBeStudentsSessionAtThisSlot(int day, int slot)
        {
            foreach (Student student in Students)
            {
                if (!student.GetIfCanBeSessionAtThisSlot(day, slot))
                {
                    return false;
                }
            }
            return true;
        }

        public bool GetIfCanBeInstructorSessionToday(int day)
        {
            foreach (Instructor instructor in Instructors)
            {
                if (!instructor.GetIfCanBeSessionsToday(day))
                {
                    return false;
                }
            }
            return true;
        }

        public bool GetIfCanBeInstructorSessionAtThisSlot(int day, int slot)
        {
            foreach (Instructor instructor in Instructors)
            {
                if (!instructor.GetIfCanBeSessionAtThisSlot(day, slot))
                {
                    return false;
                }
            }
            return true;
        }

        // I don't think this method is needed
        public override bool Equals(object? obj)
        {
            if (obj is Session session)
            {
                if (GetStudents().Count != session.GetStudents().Count)
                    return false;

                foreach (Student student in GetStudents())
                {
                    if (!session.GetStudents().Contains(student))
                        return false;
                }

                if (!session.GetInstructors().Equals(GetInstructors()))
                    return false;
            }
            else
                return false;

            return true;
        }

        // This only adds the slot the session starts in, not the all the slots that the session takes place in.
        public void AddNewSessionTime(int day, int slot)
        {
            DayAndSlotNumber dayAndSlotNumber = new DayAndSlotNumber(day, slot);
            WhenSessionsAre.Add(dayAndSlotNumber);
        }

        public Session GetSessionNotBefore()
        {
            return NotBefore;
        }

        public Session GetSessionNotAfter()
        {
            return NotAfter;
        }

        public List<DayAndSlotNumber> GetWhenSessionsAre()
        {
            return [.. WhenSessionsAre];
        }

        public void RemoveLastFromWhenSessionsAre()
        {
            WhenSessionsAre.RemoveAt(WhenSessionsAre.Count - 1);
        }

        public int GetTimesPerWeek()
        {
            return TimesPerWeek;
        }

        public bool CheckIfSessionOnDay(int day)
        {
            foreach (DayAndSlotNumber dayAndSlotNumber in WhenSessionsAre)
            {
                if (dayAndSlotNumber.GetDay() == day)
                    return true;
            }
            return false;
        }
    }
}

