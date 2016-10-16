using System;

namespace StudentDiary.Entities
{
    public class Semester
    {
        public int SemesterNumber { set; get;}
        public int YearValue { set; get; }
        public int NumberOfWeeks { set; get; }
        public DateTime StartDate { set; get; }
        public TypeOfWeek StartPartOfWeek { set; get; }
    }
}
