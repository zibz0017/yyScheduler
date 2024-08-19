# Sessions
1. Need to know who is involved in the session (teacher/therapist and student(s)). There is only one teacher/therapist. Need `String Instructor` and `List Students`.
2. Need to know how many times this is per week. Add `int TimesPerWeek`.
3. Sometimes, the same session is given with different a teacher/therapist. However, that does not make it okay for that session to go on the same day as a session of the same type with a different therapist with the same student. Add a `String SessionGroup` for type of session. If this is blank, then there is no alternative sessions of the same group.
4. Need to keep track of whether there are any time restrictions to give a particular session, and we need to know on which days those are. So we need a `List<TimeRestriction> TimeRestrictions`  and we will add one `TimeRestriction` for every day of the week.
5.  Some sessions can't be after/before a different session (at the present time, this only means right after or right before, not that one session can't be at any point in a day after some other session took place.) We need `Session NotAfter` and `Session NotBefore`.
6. Perhaps Session should keep track of when Session have been placed. We'll add `List<DayAndSlotNumber> WhenSessionsAre` to keep track of when session are. This will make it easier for backtracking (we'll be able to easily find when they were scheduled for, rather than iterating through the Calendar) as well as not putting two Sessions on the same day.
7. WhenSessionsAre List will start of empty. As Session times are placed, the List will be filled.
```cs
Session {
	String Instructor; // Point 1
	List Students; // Point 1
	int TimesPerWeek; // Point 2
	String SessionGroup; // Point 3
	List<TimeRestriction> TimeRestrictions// Point 4
	Session NotAfter; // Point 5
	Session NotBefore; // Point 5
	List<DayAndSlotNumber> WhenSessionsAre; // Point 6
}
```
## DayAndSlotNumber
1. To store a day and a slot number, we'll create a simple data structure to hold the data.
```cs
DayAndSlotNumber {
	int Day;
	int SlotNumber;

	// Setters and Getters
}
```
# TimeRestriction
1. Some sessions can only be during certain times on certain days (like lunch, which has the same restrictions every day and specials) and some sessions can only be on certain days. For each day, we'll create a new TimeRestriction.
2. Need to know the earliest and latest time a session can start and end so create `int EarlistTime` and `int LatestTime`.
3. If a session can't go on a certain day, we'll want to simply create a flag to let us know. So create `bool IfCanBeSessionsToday`.
4. If there are sessions on a given day with no restrictions, we'll just make `SessionToday` true, and set `EarliestTime` to 0 and `LatestTime` to `int.MaxValue`.
5. We will add one for every of these for every day of the week to a List in each Session.
```cs
TimeRestriction {
	bool RestrictionsToday; // Point 4
	bool IfCanBeSessionsToday; // Point 3
	int EarliestTime; // Point 2
	int LatestTime; // Point 2
}
```
# Schedule
1. Need to know which sessions are being held at which times on which days. Need `List<List<List<Session>>>`.
2. Need method to check if a specific day has a certain type of session. It would be more efficient to store this data in the sessions themselves and enter the data there and then pull it out from there.
3. We need to know how many days there are in a week that can have sessions. We need to make to make sure that the top level list (which keeps track of the days) is the correct length. Should make a simple method for retrieving that.
4. Not all days have the same amount of session slots. We need to make sure that the midlevel list (which keeps track of the session slots) is the correct length. Should make simple method for retrieving this.
```cs
static Schedule {
	static List<List<List<Session>>> Calendar; // Point 1
	
	// Point 2
	public static bool CheckIfSessionOnDay(Session Session, int DayOfWeek) {
		List<List<Session>> SessionsForDay = Calendar.get(DayOfWeek);
		for (List<Session> SessionSlot in SessionsForDay) {
			if (SessionSlot.contains(Session)) {
				return True;
			}
		}
		return False;
	}

	// Point 3
	public static int GetDaysWithSessions() {
		return Calendar.Length();
	}

	// Point 4
	public static int GetSessionsInDay(int Day) {
		return Calendar.get(Day).Length();
	}
}
```
# Student
1. Need to know the first and last name of each student. So add `String FirstName` and `String LastName`.
2. Need to know if a student comes late or leaves early on specific days. I need to keep track of these, and I need to know on which day this occurs. So I need a `List<TimeRestriction> TimeRestriction`
```cs
Student {
	String FirstName; // Point 1
	String LastName; // Point 1
	List<TimeRestriction> TimeRestrictions // Point 2
}
```
# Instructor (Teacher/Therapist)
Will be the same as Student.
# StudentGroup
1. Potentially create a way to store a group of student together. This may not be necessary, though. We could just have Session contain a List of Students.
2. If we did want to implement a StudentGroup, we would need `List<Student> Students`
```cs
StudentGroup {
	List<Student> StudentGroup;
}
```
# ScheduleCreator
1. Need some way to keep track of all the sessions that we need to schedule. Need a `List<Session> SessionsToAdd` and a `int TotalSessionsCounter` to keep track of where we are up to.
2. We need a new empty Schedule which contains the basic week structure. (We will have to figure out how to instantiate it with this data. For now I am just going to instantiate a new empty one, but that needs improvement.)
3. We need to know how many days that can have sessions there are in a week. This is already stored in Schedule.
4. We will keep track of the set Sessions in the Schedule. Perhaps the session should also keep track of where they have been put. We will have to make sure that they remain synced. This will help with backtracking. We will be able to know where the sessions are and won't have to search through the whole Calendar to find them.
5. Need to iterate through the week to try and add a Session to that day. So create `int DayOfWeekCouter`.
6. Need to check the Session to make sure it can be added on that day. We'll need to check `TimeRestriction TimeSession = Sessions.TimeRestrictions.get(DayOfWeekCounter)`
7. Two options for how to put Sessions into `SessionsToAdd` List:
	1. At the present time, the `SessionsToAdd` List will contain each session that needs to be added, an amount of times that each session needs to be added, and a List of `DayAndSlotNumbers` for when they were added. When going forward, we will check if `Session.DayAndSlotNumber.Count` equals `TimesPerWeek`. If it does, we will move on to scheduling the next Session in the `SessionsToAdd` List. If it does not (meaning, `Session.DayAndSlotNumber < TimePerWeek`. It shouldn't ever be more), we will schedule another of these Sessions. We will begin from when the next day after the last Session was Scheduled for (because every slot before then has already been checked and either already has a Session or it can't have a Session). When backtracking, if we were already onto the next Session (meaning, let's say we were scheduling the 1st Session for `SessionsToAdd[5].WhenSessionsAre[0]`) and we couldn't find a slot for it so now we need to go back to the last scheduled Session part of the previous Session (meaning, `SessionsToAdd[4].WhenSessionsAre[WhenSessionsAre.Count - 1]`) we will go to that spot on the Calendar, remove the Session from where it is currently on the Calendar, remove it from the `WhenSessionsAre` List and then look for a new spot. If we can find one, we will continue like usual. If not, we will have to backtrack further (meaning `SessionsToAdd[4].WhenSessionsAre[WhenSessionsAre.Count - 2]`) until we hopefully find something.
	   Something to keep in mind is that when going forward, once we schedule a Session in, we go to the next day to schedule the next Session in (assuming it is the same Session). When backtracking, we only go to the next Slot. This has to be factored in.
	   {Potential way to optimize would be to keep track of the Slots which were already checked. Meaning, in the example given, if `SessionsToAdd[4].WhenSessionsAre[WhenSessionsAre.Count - 1]` was scheduled for Day 5, Slot 3, and then we check all slots after that and we find that there is nowhere for it to go, when we go to the previous Session, namely `SessionsToAdd[4].WhenSessionsAre[WhenSessionsAre.Count - 2]`, we don't need to recheck anything beyond Day 3, Slot 5, because we already checked it and the way the schedule currently works, something on one day does not affect whether something can be scheduled in a specific slot on the another day.}
	2. An alternative would be to add each Session to the SessionsToAdd list once for each time that the Session needs to be scheduled. We don't need to know the amount of times Sessions need to be added. However, all of the Sessions will need to be updated for when any of these Sessions are scheduled.
## Backtracking
8. When we have a session that does not fit anywhere on the schedule, we need to back one. However, what exactly we do depends on whether this was the first scheduled Session in a Session, or not. If this was the first session, then we need to go back to the previous Session in the `SessionsToAdd` List. As such we need `SessionsCounter--`. If this is not the first Session, then we need to go to the previous session that has already been scheduled and try to find a new spot for it. We will continue from where it was already placed and try to find a new spot for it.
9. We need to know when that previous session was scheduled for, so that way we can continue from there rather than restarting from the beginning (meaning, if session 27 doesn't fit anywhere and so I go bac to session 26, if session 26 was scheduled for day 3 at slot 5, that means that no time before that worked, so I want to continue looking for a spot for session 26 beginning at day 3, slot 6). This data is currently being held in both the Schedule and in the Sessions themselves. To find it in the Schedule, I would have to start iterating backward through the Schedule which would be rather inefficient. As such, I will use the data stored in the Sessions themselves to find when the Sessions are.
10. When we do this, we have to remove the stored time that the Session is currently at from the Schedule, from the Session itself, and from any other Session in the same SessionGroup.
```cs
ScheduleCreator {
	List<Session> SessionsToAdd; // Point 1
	Schedule Schedule = new Schedule();
	int TotalSessionsCounter = 0; // Point 4
	int DayOfWeekCouter = 0; // Point 5
	Session CurrentSession;
	while (TotalSessionsCounter >= 0) { // Point 4
		CurrentSession =  SessionToAdd.get();
		for (; DayOfWeekCounter <= Schedule.GetDaysWithSessions(); DayOfWeekCounter++) {
			
		} 
	}
}
```
