using System;
using System.Collections.Generic;
using StudentDiary.Entities;
using System.Data.SqlClient;

namespace StudentDiary.Repositories
{
    public class PairTimesRepository : IPairTimesRepository
    {
        private readonly string _connString;
        private const string getAllPairTimes = "spAllPairTimes";
        private const string adjustPairTime = "spAdjustPairTime";

        public PairTimesRepository(string connString)
        {
            _connString = connString;
        }

        public IEnumerable<PairTime> GetAllPairTimes()
        {
            var pairTimes = new List<PairTime>();
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(getAllPairTimes, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var pairTime = new PairTime()
                            {
                                PairStart = (TimeSpan)reader["PairStart"],
                                PairEnd = (TimeSpan)reader["PairEnd"],
                                PairNumber = (int)reader["PairNumber"]
                            };
                            pairTimes.Add(pairTime);
                        }
                    }
                }
            }
            return pairTimes;
        }

        public void AdjustPairTime(int pairNumber, TimeSpan pairStart, TimeSpan pairEnd)
        {
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(adjustPairTime, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@pairNumber", pairNumber);
                    command.Parameters.AddWithValue("@pairStart", pairStart);
                    command.Parameters.AddWithValue("@pairEnd", pairEnd);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
