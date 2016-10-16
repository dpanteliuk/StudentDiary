using StudentDiary.Entities;
using System.Collections.Generic;

namespace StudentDiary.Repositories
{
    public interface ITeacherRepository
    {
        IEnumerable<Teacher> GetAllTeachers();
        Teacher GetTeacherById(int teacherId);
        void ModifyTeacher(int teacherId, string modifiedFirstName, string modifiedMiddleName, string modifiedLastName,
                         string modifiedDescription);
        void DeleteTeacherById(int teacherId);
        int AddNewTeacher(string firstName, string middleName, string lastName, string description);
    }
}
