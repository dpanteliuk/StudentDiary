using StudentDiary.Entities;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace StudentDiary.Repositories
{
    public class PairTypeRepository :IPairTypeRepository
    {
        private readonly string _connString;
        private const string getAllPairTypes = "spGetAllPairTypes";

        public PairTypeRepository(string connString)
        {
            _connString = connString;
        }

        public IEnumerable<PairType> GetAllPairTypes()
        {
            var pairTypes = new List<PairType>();
            using (var databaseConnection = new SqlConnection(_connString))
            {
                databaseConnection.Open();
                using (var command = new SqlCommand(getAllPairTypes, databaseConnection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var pairType = new PairType
                            {
                                TypeId = (int)reader["TypeId"],
                                TypeName = (string)reader["TypeName"],
                                TypeStringId = (string)reader["TypeStringId"]
                            };
                            pairTypes.Add(pairType);
                        }
                    }
                }
            }
            return pairTypes;
        }
    }
}
