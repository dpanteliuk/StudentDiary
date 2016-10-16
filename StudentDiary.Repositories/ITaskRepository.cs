using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentDiary.Entities;

namespace StudentDiary.Repositories
{
    interface ITaskRepository
    {
        IEnumerable<SubjectTask> GetAllTasks();
        void DeleteTaskById(int taskId);

        void ModifyTask(int taskId, int selectedSubjectId, DateTime selectedDate, string selectedTaskDescription,
            int selectedTaskPriority);

        int AddNewTask(int selectedsubjectId, int selectedTaskPriority, DateTime selectedDeadline,
            string selectedTaskDescription);

        IEnumerable<SubjectTask> GetAllTasksBySubjectId(int subjectId);
    }
}
