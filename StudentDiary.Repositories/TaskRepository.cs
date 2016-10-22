using StudentDiary.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDiary.Repositories
{
    public class TaskRepository:ITaskRepository
    {
        private readonly string _connString;
        private const string getAllTasks = "spGetAllTasks";
        private const string getAllTasksBySubjectId = "spGetAllTasksBySubjectId";
        private const string deleteTaskById = "spDeleteTaskById";
        private const string modifyTask = "spModifyTask";
        private const string addTask = "spAddTask";

        public TaskRepository(string connString)
        {
            _connString = connString;
        }

        public IEnumerable<SubjectTask> GetAllTasks()
        {
            var tasks = new List<SubjectTask>();
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(getAllTasks, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var task = new SubjectTask
                            {
                                TaskId = (int)reader["TaskId"],
                                SubjectId = (int)reader["SubjectId"],
                                TaskPriority = (int)reader["TaskPriority"],
                                DeadLineDate = (DateTime)reader["DeadLineDate"],
                                TaskDescription = (string)reader["TaskDescription"]
                            };
                            tasks.Add(task);
                        }
                    }
                }
            }
            return tasks;
        }

        public void DeleteTaskById(int taskId)
        {
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(deleteTaskById, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@taskId", taskId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void ModifyTask(int taskId, int selectedSubjectId, DateTime selectedDate, string selectedTaskDescription, int selectedTaskPriority)
        {
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(modifyTask, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@taskId", taskId);
                    command.Parameters.AddWithValue("@selectedDate", selectedDate);
                    command.Parameters.AddWithValue("@selectedTaskDescription", selectedTaskDescription);
                    command.Parameters.AddWithValue("@selectedTaskPriority", taskId);
                    command.Parameters.AddWithValue("@selectedSubjectId", selectedSubjectId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public int AddNewTask(int selectedsubjectId, int selectedTaskPriority, DateTime selectedDeadline, string selectedTaskDescription)
        {
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(addTask, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@subjectId", selectedsubjectId);
                    command.Parameters.AddWithValue("@taskPriority", selectedTaskPriority);
                    command.Parameters.AddWithValue("@deadline", selectedDeadline);
                    command.Parameters.AddWithValue("@description", selectedTaskDescription);
                    return decimal.ToInt32((decimal)command.ExecuteScalar());
                }
            }
        }

        public IEnumerable<SubjectTask> GetAllTasksBySubjectId(int subjectId)
        {
            var tasks = new List<SubjectTask>();
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(getAllTasksBySubjectId, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@subjectId", subjectId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                        //Review DP: You should use some Parser method to preventing duplication of code.
                        //Reading of data such as in the GetAllTasks method.
                            var task = new SubjectTask
                            {
                                TaskId = (int)reader["TaskId"],
                                SubjectId = (int)reader["SubjectId"],
                                TaskPriority = (int)reader["TaskPriority"],
                                DeadLineDate = (DateTime)reader["DeadLineDate"],
                                TaskDescription = (string)reader["TaskDescription"]
                            };
                            tasks.Add(task);
                        }
                    }
                }
            }
            return tasks;
        }
    }
}
