using System;
using StudentDiary.Entities;

namespace StudentDiary.Repositories
{
    interface ITimeTableRepository
    {
        WeekSchedule GetWeekByDate(DateTime date);
    }
}
