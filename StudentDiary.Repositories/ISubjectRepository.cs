using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentDiary.Entities;

namespace StudentDiary.Repositories
{
    interface ISubjectRepository
    {
        List<Subject> GetTeacherSubjects(Teacher selectedTeacher);
        List<Subject> GetAllSubjects();

        Dictionary<int, SubjectScheduleItem> GetAllSubjectItemsInCertainDay(string dayName, int year, int semesterNumber,
            int weekNumber);

        Subject GetSubjectById(int subjectId);
        void RemoveAllTimeTableSubjectsFromDay(DaysOfWeek dayToRemove, int year, int semesterNumber, int weekNumber);

        void RemoveOneTimeTableSubject(int year, int semesterNumber, int weekNumber, DaysOfWeek selectedDay,
            int pairNumber);

        void AddModifyOneTimeTableSubject(int year, int semesterNumber, int weekNumber, DaysOfWeek selectedDay,
            int pairNumber, Subject selectedSubject, PairType selectedPairType);

        void DeleteSubjectById(int selectedSubjectSubjectId);
        int AddNewSubject(string subjectName, int teacherId, string description);
        void ModifySubject(int subjectId, string subjectName, string subjectDescription, int subjectTeacherId);
    }
}
