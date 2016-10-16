using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentDiary.UI.UICommands
{
    public class TaskCommands
    {
        static TaskCommands()
        {
            CmdDeleteTask = new RoutedUICommand("Delete selected task", "CmdDeleteTask", typeof(TaskCommands));
            CmdModifyTask = new RoutedUICommand("Modify selected task", "CmdModifyTask", typeof(TaskCommands));
        }
        public static RoutedUICommand CmdModifyTask
        {
            get; set;
        }
        public static RoutedUICommand CmdDeleteTask
        {
            get; set;
        }
    }
}
