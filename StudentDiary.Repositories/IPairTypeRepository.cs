using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentDiary.Entities;

namespace StudentDiary.Repositories
{
    interface IPairTypeRepository
    {
        IEnumerable<PairType> GetAllPairTypes();
    }
}
