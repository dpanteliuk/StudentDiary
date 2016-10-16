using System;
using System.Collections.Generic;
using System.Linq;
using StudentDiary.Entities;
using System.Data.SqlClient;

namespace StudentDiary.Repositories
{
    public class TimetableRepository:ITimeTableRepository
    {
        private readonly string _connString;

        readonly SubjectRepository subjectRepository;

        private const string getPairTimes = "spGetPairTimes";
        private const string getWeekDays = "spGetWeekDays";
        public TimetableRepository(string connString)
        {
            _connString = connString;
            subjectRepository = new SubjectRepository(connString);
        }

        public WeekSchedule GetWeekByDate(DateTime date)
        {
            WeekSchedule schedule = new WeekSchedule
            {
                ScheduleDayColumns = new List<Entities.DayOfWeek>()
            };
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                
                schedule.Year = date.Year;
                schedule.SemesterNumber = (date.Month >= 9) ? 1 : 2;

                int numberOfWeeksInSemester;
                DateTime semesterStartDate;
                int semesterStartPartOfWeek;

                using (var scheduleInfoCommand = new SqlCommand("SELECT TOP 1 StartDate,NumberOfWeeks,StartPartOfWeekId FROM tblSemester WHERE YearValue = @Year AND SemesterNumber = @SemesterNumber", databaseConnection))
                {
                    scheduleInfoCommand.Parameters.AddWithValue("@Year", schedule.Year);
                    scheduleInfoCommand.Parameters.AddWithValue("@SemesterNumber", schedule.SemesterNumber);
                        using (var reader = scheduleInfoCommand.ExecuteReader())
                        {
                            if (! reader.HasRows)
                            {
                                throw new ArgumentException("There is no such semester in database");
                            }
                            reader.Read();
                            numberOfWeeksInSemester = (int)reader["NumberOfWeeks"];
                            semesterStartDate = (DateTime)reader["StartDate"];
                            semesterStartPartOfWeek = (int)reader["StartPartOfWeekId"];
                        }
                }
                if (date < semesterStartDate)
                {
                    throw new ArgumentException("Date is out of studing period");
                }

                schedule.WeekNumber = (int) Math.Ceiling((double) (date - semesterStartDate).Days/7);
                if (schedule.WeekNumber == 0)
                {
                    schedule.WeekNumber = 1;
                }
                if (schedule.WeekNumber > numberOfWeeksInSemester)
                {
                    throw new ArgumentException("Date is out of studing period");
                }
                schedule.WeekType = (schedule.WeekNumber - semesterStartPartOfWeek) % 2 == 0
                    ? semesterStartPartOfWeek
                    : (semesterStartPartOfWeek == 1 ? 2 : 1);

                using (var command = new SqlCommand(getWeekDays, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            schedule.ScheduleDayColumns.Add(new Entities.DayOfWeek
                            {
                                DayName = (DaysOfWeek)Enum.Parse(typeof(DaysOfWeek), reader["Name"].ToString()),
                                SubjectDayDict = new Dictionary<int, SubjectScheduleItem>()
                            });
                        }
                    }
                }
                foreach (var day in schedule.ScheduleDayColumns)
                {
                    var dayName = day.DayName.ToString();
                    day.SubjectDayDict = subjectRepository.GetAllSubjectItemsInCertainDay(dayName, schedule.Year,
                        schedule.SemesterNumber, schedule.WeekNumber);
                }

                schedule.ScheduleDayColumns.RemoveAll(
                    dayColumn =>
                        (dayColumn.SubjectDayDict.Count == 0) ||
                        dayColumn.SubjectDayDict.All(item => item.Value == null));
            }
            schedule.ScheduleDayColumns.Sort();
            return schedule;
        }
    }
}
