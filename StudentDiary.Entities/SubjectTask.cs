using System;

namespace StudentDiary.Entities
{
    public class SubjectTask
    {
        public int TaskId { set; get; }
        public int SubjectId { set; get; }
        public string TaskDescription { set; get; }
        public DateTime DeadLineDate { set; get; }
        public int TaskPriority { set; get; }
    }
}
