using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentDiary.Entities;
using System.Data.SqlClient;

namespace StudentDiary.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly string _connString;
        private const string getTeachers = "spGetAllTeachers";
        private const string getTeacher = "spGetTeacher";
        private const string deleteTeacher = "spDeleteTeacher";
        private const string addTeacher = "spAddTeacher";
        private const string modifyTeacher = "spModifyTeacher";
        public TeacherRepository(string connString)
        {
            _connString = connString;
        }
        public IEnumerable<Teacher> GetAllTeachers()
        {
            var teachers = new List<Teacher>();
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(getTeachers, databaseConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var teacher = new Teacher
                            {
                                TeacherId = (int)reader["TeacherId"],
                                FirstName = (string)reader["FirstName"],
                                MiddleName = (string)reader["MiddleName"],
                                LastName = (string)reader["LastName"],
                                TeacherDescription = reader["TeacherDescription"]==DBNull.Value ? "" : (string)reader["TeacherDescription"]
                            };
                            teachers.Add(teacher);
                        }
                    }
                }
            }
            return teachers;
        }

        public void ModifyTeacher(int teacherId,string modifiedFirstName,string modifiedMiddleName,string modifiedLastName,string modifiedDescription)
        {
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(modifyTeacher, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TeacherId", teacherId);
                    command.Parameters.AddWithValue("@ModifiedFirstName", modifiedFirstName);
                    command.Parameters.AddWithValue("@ModifiedMiddleName", modifiedMiddleName);
                    command.Parameters.AddWithValue("@ModifiedLastName", modifiedLastName);
                    command.Parameters.AddWithValue("@ModifiedDescription", modifiedDescription);
                    command.ExecuteNonQuery();
                }
            }
        }

        public int AddNewTeacher(string firstName, string middleName, string lastName, string description)
        {
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(addTeacher, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@MiddleName", middleName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@Description", description);
                    return decimal.ToInt32((decimal)command.ExecuteScalar());
                }
            }
        }

        public Teacher GetTeacherById(int teacherId)
        {
            Teacher teacher;
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(getTeacher, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@teacherId", teacherId);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        teacher = new Teacher
                        {
                            TeacherId = (int)reader["TeacherId"],
                            FirstName = (string)reader["FirstName"],
                            MiddleName = (string)reader["MiddleName"],
                            LastName = (string)reader["LastName"],
                            TeacherDescription =
                                reader["TeacherDescription"] == DBNull.Value
                                    ? ""
                                    : (string) reader["TeacherDescription"]
                        };
                    }
                }
            }
            return teacher;
        }

        public void DeleteTeacherById(int teacherId)
        {
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(deleteTeacher, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@teacherId", teacherId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
