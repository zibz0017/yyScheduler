namespace ConsoleApp1.Classes
{
    public class DayAndSlotNumber
    {
        int Day;
        int SlotNumber;

        public DayAndSlotNumber(int day, int slotNumber)
        {
            Day = day;
            SlotNumber = slotNumber;
        }

        public int GetDay()
        {
            return Day;
        }

        public int GetSlotNumber()
        {
            return SlotNumber;
        }
    }
}
