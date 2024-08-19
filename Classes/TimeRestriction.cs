using System.Numerics;

namespace ConsoleApp1.Classes
{
    public class TimeRestriction
    {
        bool IfCanBeSessionsToday; // Point 3
        int EarliestSlot = 0; // Point 2
        int LatestSlot = int.MaxValue; // Point 2

        public TimeRestriction(bool ifCanBeSessionsToday)
        {
            IfCanBeSessionsToday = ifCanBeSessionsToday;
        }

        public TimeRestriction(bool ifCanBeSessionsToday, int earliestSlot, int latestSlot)
        {
            IfCanBeSessionsToday= ifCanBeSessionsToday;
            EarliestSlot = earliestSlot;
            LatestSlot = latestSlot;
        }

        public bool GetIfCanBeSessionsToday()
        {
            return IfCanBeSessionsToday;
        }

        public bool GetIfCanBeSessionAtThisSlot(int slot)
        {
            return slot >= EarliestSlot && slot <= LatestSlot;
        }
    }
}
