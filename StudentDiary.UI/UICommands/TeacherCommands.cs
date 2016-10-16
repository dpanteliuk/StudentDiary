using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentDiary.UI.UICommands
{
    public class TeacherCommands
    {
        static TeacherCommands()
        {
            CmdDeleteTeacher = new RoutedUICommand("Delete selected Teacher", "CmdDeleteTeacher", typeof(TeacherCommands));
            CmdModifyTeacher = new RoutedUICommand("Modify selected Teacher", "CmdModifyTeacher", typeof(TeacherCommands));
        }
        public static RoutedUICommand CmdDeleteTeacher
        {
            get; set;
        }

        public static RoutedUICommand CmdModifyTeacher
        {
            get; set;
        }
    }
}
