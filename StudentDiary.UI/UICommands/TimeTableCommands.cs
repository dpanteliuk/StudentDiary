using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentDiary.UI.UICommands
{
    public class TimeTableCommands
    {
        static TimeTableCommands()
        {
            CmdDeleteTimeTableSubject = new RoutedUICommand("Delete selected subject from TimeTable", "CmdDeleteTimeTableSubject", typeof(TimeTableCommands));
            CmdModifyTimeTableSubject = new RoutedUICommand("Modify selected subject from TimeTable", "CmdModifyTimeTableSubject", typeof(TimeTableCommands));
            CmdChangeTimes = new RoutedUICommand("Change selected times", "CmdChangeTimes", typeof(TimeTableCommands));
            CmdTimeTableGetSubjectInfo = new RoutedUICommand("Get subject information", "CmdTimeTableGetSubjectInfo", typeof(TimeTableCommands));
            CmdTimeTableGetTeacherInfo = new RoutedUICommand("Get teacher information", "CmdTimeTableGetTeacherInfo", typeof(TimeTableCommands));
            CmdTimeTableGetTasksInfo = new RoutedUICommand("Get tasks information", "CmdTimeTableGetTasksInfo", typeof(TimeTableCommands));
        }
        public static RoutedUICommand CmdDeleteTimeTableSubject
        {
            get; set;
        }
        public static RoutedUICommand CmdModifyTimeTableSubject
        {
            get; set;
        }
        public static RoutedUICommand CmdTimeTableGetSubjectInfo
        {
            get; set;
        }
        public static RoutedUICommand CmdTimeTableGetTeacherInfo
        {
            get; set;
        }
        public static RoutedUICommand CmdTimeTableGetTasksInfo
        {
            get; set;
        }
        public static RoutedUICommand CmdChangeTimes
        {
            get; set;
        }
    }
}
