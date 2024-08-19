namespace ConsoleApp1.Classes
{
    public class ScheduleCreator
    {
        List<Session> SessionsToAdd; // Point 1
        int SessionsCounter = 0; // Point 4
        int DayOfWeekCounter; // Point 5
        int SlotCounter;
        Session CurrentSession;
        bool FoundSlotForSession = true; // This basically means are we going forward, or are we backtracking
        // when set to true, we are going forward, and when it is false, we are backtracking.
        bool FoundSolution = false;
        public void CreateSchedule()
        {
            while (SessionsCounter >= 0)
            { // Point 4
                CurrentSession = SessionsToAdd[SessionsCounter];
                // If we are going forward (not backtracking)
                if (FoundSlotForSession)
                {
                    // If we have already found a slot for all of these sessions
                    if (CurrentSession.GetWhenSessionsAre().Count == CurrentSession.GetTimesPerWeek())
                    {
                        // If we have found a slot for every session
                        if (SessionsCounter == SessionsToAdd.Count)
                            FoundSlotForSession = false;
                        else
                        {
                            CurrentSession = SessionsToAdd[++SessionsCounter];
                            DayOfWeekCounter = 0;
                        }
                    }
                    // If we have not found a slot for all of these sessions
                    else
                    {
                        // Start from the beginning of the next day (because we can't have 2 of the same sessions
                        // on the same day).
                        DayAndSlotNumber dayAndSlotNumber = CurrentSession.GetWhenSessionsAre().Last();
                        DayOfWeekCounter = dayAndSlotNumber.GetDay() + 1;
                    }
                    SlotCounter = 0;
                }
                // If we are backtracking
                if (!FoundSlotForSession)
                {
                    if (CurrentSession.GetWhenSessionsAre().Count == 0)
                    {
                        if (SessionsCounter > 0)
                            CurrentSession = SessionsToAdd[--SessionsCounter];
                        else
                            SearchCompleted(FoundSolution);
                    }
                    DayAndSlotNumber dayAndSlotNumber = CurrentSession.GetWhenSessionsAre().Last();
                    CurrentSession.RemoveLastFromWhenSessionsAre();
                    Schedule.RemoveSessionsFromSlots(CurrentSession, dayAndSlotNumber.GetDay(),
                        dayAndSlotNumber.GetSlotNumber());
                    DayOfWeekCounter = dayAndSlotNumber.GetDay();
                    SlotCounter = dayAndSlotNumber.GetSlotNumber() + 1;
                }
                for (DayOfWeekCounter = 0; DayOfWeekCounter < Schedule.GetDaysWithSessions(); DayOfWeekCounter++)
                {
                    // See if same session is already scheduled for this day (includes checking for session of
                    // same session group). This is currently very inefficient. We should have a way for the data
                    // of when sessions are stored in the sessions themselves and make searching for it very simple
                    // rather than iterating through the whole day in the Schedule. This would have to also make
                    // sure that sessions of the same group know to watch out for each other.
                    if (Schedule.CheckIfSessionOnDay(CurrentSession, DayOfWeekCounter))
                        continue;
                    // See if session can be scheduled for this day
                    if (!CurrentSession.GetIfCanBeSessionsToday(DayOfWeekCounter))
                        continue;
                    // See if all students can have a session for this day
                    if (!CurrentSession.GetIfCanBeStudentsSessionToday(DayOfWeekCounter))
                        continue;
                    // See if instructor can have a session for this day
                    if (!CurrentSession.GetIfCanBeInstructorSessionToday(DayOfWeekCounter))
                        continue;
                    int PossibleSlotsToScheduleSession = Schedule.GetSlotsInDay(DayOfWeekCounter) -
                        (CurrentSession.GetLengthOfSessions() - 1);
                    for (; SlotCounter < PossibleSlotsToScheduleSession; SlotCounter++)
                    {
                        // The day is really divided into 30 min slots. Because some sessions are 15 or 45 minutes
                        // we make the slots 15 minutes. However, 30 minute sessions can only be scheduled on the
                        // half hour or on the hour.
                        if (SlotCounter % 2 != 0 && CurrentSession.GetLengthOfSessions() == 2)
                            continue;
                        // See if session can be scheduled for this time
                        if (!CurrentSession.GetIfCanBeSessionAtThisSlot(DayOfWeekCounter, SlotCounter))
                            continue;
                        // See if students can have session for this time
                        if (!CurrentSession.GetIfCanBeStudentsSessionAtThisSlot(DayOfWeekCounter, SlotCounter))
                            continue;
                        // See if instructor can have session for this time
                        if (!CurrentSession.GetIfCanBeInstructorSessionAtThisSlot(DayOfWeekCounter, SlotCounter))
                            continue;
                        // See if student already has session scheduled for this time
                        if (Schedule.GetIfStudentOrInstructorHasSessionAtSlot(CurrentSession, DayOfWeekCounter, SlotCounter))
                            continue;
                        // Check if session can't go after or before a different session
                        if (Schedule.CheckIfSessionCanBeAfterOrBeforeSpecificSession(CurrentSession, DayOfWeekCounter, SlotCounter))
                            continue;
                        // Check for anything else that needs checking (can't think of anything else right now)
                        // Add session to slot, change FoundSlotForSession, and leave loop,
                        Schedule.AddSessionsToSlots(DayOfWeekCounter, SlotCounter, SessionsToAdd[SessionsCounter]);
                        CurrentSession.AddNewSessionTime(DayOfWeekCounter, SlotCounter);
                        FoundSlotForSession = true;
                        break;
                    }
                    // If we found a slot for the session, we can leave the loop
                    if (FoundSlotForSession)
                        break;
                    // If not, we will go to the next day to find a slot
                }
                // If we did not find any spot for the session
                FoundSlotForSession = false;
            }
        }
        private static void SearchCompleted(bool success)
        {
            if (success)
            {
                // some sort of success response
            }
            else
            {
                // some sort of failure response
            }
        }
    }
}
