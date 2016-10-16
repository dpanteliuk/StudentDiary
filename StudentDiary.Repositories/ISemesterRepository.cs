using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentDiary.Entities;

namespace StudentDiary.Repositories
{
    interface ISemesterRepository
    {
        Semester GetSemesterByDate(DateTime date);
        IEnumerable<Semester> GetAllSemesters();

        void AddNewSemester(int selectedSemesterYear, int selectedSemesterNumber, DateTime selectedSemesterStartDate,
            int selectedSemesterNumberOfWeeks, TypeOfWeek selectedStartPartOfWeek);

        void DeleteSemester(Semester semesterToDelete);
    }
}
