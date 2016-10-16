using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentDiary.Entities;

namespace StudentDiary.Repositories
{
    interface IPairTimesRepository
    {
        IEnumerable<PairTime> GetAllPairTimes();
        void AdjustPairTime(int pairNumber, TimeSpan pairStart, TimeSpan pairEnd);
    }
}
