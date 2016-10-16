using StudentDiary.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDiary.Repositories
{
    public class SubjectRepository :ISubjectRepository
    {
        private readonly string _connString;
        private const string getSubjectsByTeacherId = "spGetSubjectsByTeacherId";
        private const string getSubjectsInCertainDay = "spGetSubjectsInCertainDay";
        private const string getAllSubjects = "spGetAllSubjects";
        private const string getSubjectById = "spGetSubjectById";
        private const string removeSubjectsInCertainDay = "spRemoveSubjectsInCertainDay";
        private const string removeCertainSubject = "spRemoveCertainSubject";
        private const string addModifyCertainSubject = "spAddModifyCertainSubject";
        private const string deleteSubjectById = "spDeleteSubjectById";
        private const string addNewSubject = "spAddNewSubject";
        private const string modifySubject = "spModifySubject";

        public SubjectRepository(string connString)
        {
            _connString = connString;
        }

        public List<Subject> GetTeacherSubjects(Teacher selectedTeacher)
        {
            var subjects = new List<Subject>();
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(getSubjectsByTeacherId, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@teacherId", selectedTeacher.TeacherId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var subject = new Subject
                            {
                                SubjectId = (int)reader["SubjectId"],
                                Name = (string)reader["Name"],
                                SubjectDescription = reader["Subjectdescription"] == DBNull.Value
                                                     ? "" 
                                                     : (string)reader["Subjectdescription"],
                                TeacherId = (int)reader["TeacherId"],
                            };
                            subjects.Add(subject);
                        }
                    }
                }
            }
            return subjects;
        }
        public List<Subject> GetAllSubjects()
        {
            var subjects = new List<Subject>();
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(getAllSubjects, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var subject = new Subject
                            {
                                SubjectId = (int)reader["SubjectId"],
                                Name = (string)reader["Name"],
                                SubjectDescription = reader["Subjectdescription"] == DBNull.Value 
                                                     ? "" 
                                                     : (string)reader["Subjectdescription"],
                                TeacherId = reader["TeacherId"] == DBNull.Value ? -1:(int)reader["TeacherId"],
                            };
                            subjects.Add(subject);
                        }
                    }
                }
            }
            return subjects;
        }

        public Dictionary<int, SubjectScheduleItem> GetAllSubjectItemsInCertainDay(string dayName, int year, int semesterNumber, int weekNumber)
        {
            var subjects = new Dictionary<int, SubjectScheduleItem>();
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(getSubjectsInCertainDay, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@dayName", dayName);
                    command.Parameters.AddWithValue("@year", year);
                    command.Parameters.AddWithValue("@semesterNumber", semesterNumber);
                    command.Parameters.AddWithValue("@weekNumber", weekNumber);
                    using (var reader = command.ExecuteReader())
                    {
                            while (reader.Read())
                            {
                                if (!reader.HasRows)
                                {
                                    continue;
                                }
                                SubjectScheduleItem subjectItem = (reader["SubjectId"] != DBNull.Value && reader["TeacherId"] != DBNull.Value)
                                    ? new SubjectScheduleItem ()
                                    {
                                        Subject = new Subject
                                        {
                                            SubjectId = (int) reader["SubjectId"],
                                            Name = (string) reader["Name"],
                                            SubjectDescription = (reader["Subjectdescription"] == DBNull.Value) ? "": (string) reader["Subjectdescription"],
                                            TeacherId = (int) reader["TeacherId"]
                                        },
                                        PairType = new PairType ()
                                        {
                                            TypeId = (int) reader["PairTypeId"],
                                            TypeName = (string) reader["TypeName"],
                                            TypeStringId = (string) reader["TypeStringId"]
                                        }
                                    }
                                    : null;
                                subjects.Add((int) reader["PairNumber"], subjectItem);
                            }
                    }
                }
                if (subjects.Count != 0)
                {
                    while (subjects.Count > 0 && subjects[subjects.Keys.Max()] == null)
                    {
                        subjects.Remove(subjects.Keys.Max());
                    }
                }    
            }
            return subjects;
        }

        public Subject GetSubjectById(int subjectId)
        {
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(getSubjectById, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@subjectId", subjectId);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        return new Subject
                        {
                            SubjectId = (int) reader["SubjectId"],
                            Name = (string) reader["Name"],
                            SubjectDescription = reader["Subjectdescription"] == DBNull.Value
                                ? ""
                                : (string) reader["Subjectdescription"],
                            TeacherId = reader["TeacherId"]!= DBNull.Value?(int) reader["TeacherId"]:-1
                        };
                    }
                }
            }
        }

        public void RemoveAllTimeTableSubjectsFromDay(DaysOfWeek dayToRemove, int year, int semesterNumber, int weekNumber)
        {
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(removeSubjectsInCertainDay, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@dayName", dayToRemove.ToString());
                    command.Parameters.AddWithValue("@year", year);
                    command.Parameters.AddWithValue("@semesterNumber", semesterNumber);
                    command.Parameters.AddWithValue("@weekNumber", weekNumber);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void RemoveOneTimeTableSubject(int year, int semesterNumber, int weekNumber, DaysOfWeek selectedDay, int pairNumber)
        {
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(removeCertainSubject, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@year", year);
                    command.Parameters.AddWithValue("@semesterNumber", semesterNumber);
                    command.Parameters.AddWithValue("@weekNumber", weekNumber);
                    command.Parameters.AddWithValue("@dayName", selectedDay.ToString());
                    command.Parameters.AddWithValue("@pairNumber", pairNumber);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddModifyOneTimeTableSubject(int year, int semesterNumber, int weekNumber, DaysOfWeek selectedDay, int pairNumber, Subject selectedSubject, PairType selectedPairType)
        {
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(addModifyCertainSubject, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@year", year);
                    command.Parameters.AddWithValue("@semesterNumber", semesterNumber);
                    command.Parameters.AddWithValue("@weekNumber", weekNumber);
                    command.Parameters.AddWithValue("@dayName", selectedDay.ToString());
                    command.Parameters.AddWithValue("@pairNumber", pairNumber);
                    command.Parameters.AddWithValue("@selectedSubjectId", selectedSubject.SubjectId);
                    command.Parameters.AddWithValue("@selectedSubjectPairTypeId", selectedPairType.TypeId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteSubjectById(int selectedSubjectSubjectId)
        {
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(deleteSubjectById, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@subjectId", selectedSubjectSubjectId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public int AddNewSubject(string subjectName, int teacherId, string description)
        {
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(addNewSubject, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@subjectName", subjectName);
                    command.Parameters.AddWithValue("@teacherId", teacherId);
                    command.Parameters.AddWithValue("@description", description);
                    return decimal.ToInt32((decimal) command.ExecuteScalar());
                }
            }
        }

        public void ModifySubject(int subjectId, string subjectName, string subjectDescription, int subjectTeacherId)
        {
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(modifySubject, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", subjectId);
                    command.Parameters.AddWithValue("@subjectName", subjectName);
                    command.Parameters.AddWithValue("@teacherId", subjectTeacherId);
                    command.Parameters.AddWithValue("@description", subjectDescription);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
