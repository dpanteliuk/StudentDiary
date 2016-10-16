using StudentDiary.Entities;
using System.Data.SqlClient;

namespace StudentDiary.Repositories
{
    public class UserRepository : IUserRepository
    {
        private const string loginUser = "spLoginUser";
        private readonly string _connString;

        public UserRepository(string connString)
        {
            _connString = connString;
        }

        public User Login(string writtenLogin, string pass)
        {
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(loginUser, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@login", writtenLogin);
                    command.Parameters.AddWithValue("@pass", pass);
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            return null;
                        }
                        reader.Read();
                        return new User
                        {
                            Name = (string)reader["Name"],
                            Login = (string)reader["Login"],
                            UserId = (int)reader["UserId"]
                        };
                    }
                }
            }
        }
    }
}
