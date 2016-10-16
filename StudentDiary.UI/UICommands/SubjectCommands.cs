using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentDiary.UI.UICommands
{
    public class SubjectCommands
    {
        static SubjectCommands()
        {
            CmdGetSubjectTeacher = new RoutedUICommand("Get selected subject teacher", "CmdGetSubjectTeacher", typeof(SubjectCommands));
            CmdGetSubjectTasks = new RoutedUICommand("Get selected subject tasks", "CmdGetSubjectTasks", typeof(SubjectCommands));
            CmdDeleteSubject = new RoutedUICommand("Delete selected subject", "CmdDeleteSubject", typeof(SubjectCommands));
            CmdModifySubject = new RoutedUICommand("Modify selected subject", "CmdModifySubject", typeof(SubjectCommands));
        }
        public static RoutedUICommand CmdGetSubjectTeacher
        {
            get; set;
        }
        public static RoutedUICommand CmdGetSubjectTasks
        {
            get; set;
        }
        public static RoutedUICommand CmdDeleteSubject
        {
            get; set;
        }
        public static RoutedUICommand CmdModifySubject
        {
            get; set;
        }
    }
}
