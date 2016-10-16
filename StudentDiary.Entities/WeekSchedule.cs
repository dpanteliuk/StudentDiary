using System;
using System.Collections.Generic;

namespace StudentDiary.Entities
{
    public class WeekSchedule
    {
        public List<DayOfWeek> ScheduleDayColumns { set; get; }
        public int WeekNumber { set; get; }
        public int Year { set; get; }
        public int SemesterNumber { set; get; }
        public int WeekType { set; get; }
    }

    public class DayOfWeek : IComparable<DayOfWeek>
    {
        public DaysOfWeek DayName { set; get; }
        public Dictionary<int, SubjectScheduleItem> SubjectDayDict { set; get; }

        public int CompareTo(DayOfWeek other)
        {
            return DayName.CompareTo(other.DayName);
        }
    }

    public class SubjectScheduleItem
    {
        public Subject Subject { set; get; }
        public PairType PairType { set; get; }
    }
}
