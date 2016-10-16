using System;
using System.Collections.Generic;
using StudentDiary.Entities;
using System.Data.SqlClient;

namespace StudentDiary.Repositories
{
    public class SemesterRepository : ISemesterRepository
    {
        private readonly string _connString;
        private const string getSemester = "spGetSemester";
        private const string getAllSemesters = "spGetAllSemesters";
        private const string addNewSemester = "spAddNewSemester";
        private const string deleteSemester = "spDeleteSemester";

        public SemesterRepository(string connString)
        {
            _connString = connString;
        }

        public Semester GetSemesterByDate(DateTime date)
        {
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(getSemester, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@YearValue",date.Year);
                    command.Parameters.AddWithValue("@SemesterNumber",(date.Month >= 9) ? 1 : 2);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        if (!reader.HasRows)
                        {
                            throw new ArgumentException("No semester awailable");
                        }
                        return new Semester()
                        {
                            NumberOfWeeks = (int)reader["NumberOfWeeks"],
                            StartDate = (DateTime)reader["StartDate"],
                            YearValue = (int)reader["YearValue"],
                            SemesterNumber = (int)reader["SemesterNumber"],
                            StartPartOfWeek = (TypeOfWeek)((int)reader["StartPartOfWeekId"] - 1)
                        };
                    }
                }
            }
        }

        public IEnumerable<Semester> GetAllSemesters()
        {
       
            var semesters = new List<Semester>();
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(getAllSemesters, databaseConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var semester = new Semester
                            {
                                SemesterNumber = (int)reader["SemesterNumber"],
                                YearValue = (int)reader["YearValue"],
                                NumberOfWeeks = (int)reader["NumberOfWeeks"],
                                StartDate = (DateTime)reader["StartDate"],
                                StartPartOfWeek = (TypeOfWeek)((int)reader["StartPartOfWeekId"] - 1)
                            };
                            semesters.Add(semester);
                        }
                    }
                }
            }
            return semesters;   
        }

        public void AddNewSemester(int selectedSemesterYear, int selectedSemesterNumber, DateTime selectedSemesterStartDate, int selectedSemesterNumberOfWeeks, TypeOfWeek selectedStartPartOfWeek)
        {
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(addNewSemester, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@year", selectedSemesterYear);
                    command.Parameters.AddWithValue("@number", selectedSemesterNumber);
                    command.Parameters.AddWithValue("@startDate", selectedSemesterStartDate);
                    command.Parameters.AddWithValue("@numberOfWeeks", selectedSemesterNumberOfWeeks);
                    command.Parameters.AddWithValue("@startPartOfWeek", (int)selectedStartPartOfWeek+1);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteSemester(Semester semesterToDelete)
        {
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(deleteSemester, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@year", semesterToDelete.YearValue);
                    command.Parameters.AddWithValue("@number", semesterToDelete.SemesterNumber);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
