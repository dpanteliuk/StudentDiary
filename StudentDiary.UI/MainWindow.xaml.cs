using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Configuration;
using StudentDiary.Repositories;
using StudentDiary.Entities;
using System.Data.SqlClient;
using StudentDiary.UI.HelperClasses;

namespace StudentDiary.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly string _connectionString;

        private readonly TeacherRepository _teacherRepository;
        private readonly SubjectRepository _subjectRepository;
        private readonly TimetableRepository _timetableRepository;
        private readonly PairTimesRepository _pairTimesRepository;
        private readonly TaskRepository _taskRepository;
        private readonly SemesterRepository _semesterRepository;
        private readonly PairTypeRepository _pairTypeRepository;

        WeekSchedule _displayedSchedule;
        List<PairTime> _displayedPairTimes;
        List<Teacher> _displayedTeachers;
        List<Subject> _displayedSubjects;
        List<TaskTableItem> _displayedTaskTableItems;
        List<int> _displayedYears;
        DateTime _displayedDate;
        Semester _displayedSemester;
        List<Semester> _allSemesters;


        public MainWindow()
        {
            InitializeComponent();

            var loginDialog = new Dialogs.LoginDialog();
            loginDialog.ShowDialog();

            if (!loginDialog.IsSelected)
            {
                Close();
            }

            _connectionString = ConfigurationManager.ConnectionStrings["StudentDiaryConnectionString"].ConnectionString;

            _teacherRepository = new TeacherRepository(_connectionString);
            _timetableRepository = new TimetableRepository(_connectionString);
            _subjectRepository = new SubjectRepository(_connectionString);
            _pairTimesRepository = new PairTimesRepository(_connectionString);
            _taskRepository = new TaskRepository(_connectionString);
            _semesterRepository = new SemesterRepository(_connectionString);
            _pairTypeRepository = new PairTypeRepository(_connectionString);

            InitSemester();
            InitSubjectTimes();
            //InitSubjectControl();
            InitTeachersList();
            InitSubjectList();
            InitTaskGrid();

            GeneralMenu.SelectedIndex = 0;
        }

        private void InitSemester()
        {
            _allSemesters = _semesterRepository.GetAllSemesters().ToList();
            Semesters.ItemsSource = _allSemesters;

            if (_allSemesters.Count == 0)
            {
                OutOfSemestersHandler();
                return;
            }
            try
            {
                _displayedSemester = _semesterRepository.GetSemesterByDate(DateTime.Now);
            }
            catch (ArgumentException)
            {
                _displayedSemester = _allSemesters[0];
            }

            Semesters.SelectedItem =
                _allSemesters.First(
                    sem =>
                        (sem.SemesterNumber == _displayedSemester.SemesterNumber) &&
                        sem.YearValue == _displayedSemester.YearValue);

        }

        private void OutOfSemestersHandler()
        {
            var result =
                MessageBox.Show(
                    "There are no semesters left.\n In order to continue using StudentDiary you have to create a new one",
                    "Warning", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                Close();
            }
            CreateNewSemester(new object(), new AccessKeyPressedEventArgs());
            Semesters.SelectedIndex = 0;
            //var addSemesterDialog = new AddSemesterDialog(new List<Semester>());
        }

        private void InitSubjectList()
        {
            _displayedSubjects = new List<Subject>();
            foreach (Subject subject in _subjectRepository.GetAllSubjects())
            {
                _displayedSubjects.Add(subject);
            }
            SubjectList.ItemsSource = _displayedSubjects;
            AwailableSubjects.ItemsSource = _displayedSubjects;
        }

        private void InitTeachersList()
        {
            _displayedTeachers = new List<Teacher>();
            foreach (Teacher teacher in _teacherRepository.GetAllTeachers())
            {
                _displayedTeachers.Add(teacher);
            }
            TeachersList.ItemsSource = _displayedTeachers;
        }

        private void InitSubjectControl()
        {
            if (_displayedSchedule==null)
            {
                try
                {
                    _displayedSchedule = _timetableRepository.GetWeekByDate(DateTime.Now);
                    _displayedDate = DateTime.Now;
                    Semesters.SelectedItem =
                        _allSemesters.First(
                            sem =>
                                sem.YearValue == _displayedDate.Year && sem.SemesterNumber == _displayedSchedule.SemesterNumber);
                }
                catch (ArgumentException)
                {
                    _displayedSchedule = _timetableRepository.GetWeekByDate(_displayedSemester.StartDate);
                    _displayedDate = _displayedSemester.StartDate;
                }
            }
            else
            {
                _displayedSchedule = _timetableRepository.GetWeekByDate(_displayedSemester.StartDate);
                _displayedDate = _displayedSemester.StartDate;
            }
            SubjectControl.DayClickHandler = SubjSelectionChanged;
            SubjectControl.LoadSchedule(_displayedSchedule);
            RefreshWeekStats();
        }

        private void SubjSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedScheduleItem = SubjectControl.GetSelectedSubjectScheduleItem();
            if (selectedScheduleItem == null)
            {
                TimeTableDescriptionType.Text = "";
                TimeTableDescriptionName.Text = "";
                TimeTableDescriptionTeacher.Text = "";
                TimeTableDescriptionNuberOfTasks.Text = "";
                return;
            }
            var selectedItemTeacher =
                _displayedTeachers.First(teach => teach.TeacherId == selectedScheduleItem.Subject.TeacherId);
            TimeTableDescriptionType.Text = "Type of the subject: "+ selectedScheduleItem.PairType.TypeName;
            TimeTableDescriptionName.Text = "Subject Name: "+selectedScheduleItem.Subject.Name;
            TimeTableDescriptionTeacher.Text = "Subject Teacher: "+selectedItemTeacher.FirstName+" "+selectedItemTeacher.MiddleName;
            TimeTableDescriptionNuberOfTasks.Text = "Count of tasks: "+_displayedTaskTableItems.Count(item=>item.TaskTableItemSubject.SubjectId == selectedScheduleItem.Subject.SubjectId).ToString();
        }


        private void InitSubjectTimes()
        {
            _displayedPairTimes = new List<PairTime>();
            foreach (PairTime pairTime in _pairTimesRepository.GetAllPairTimes())
            {
                _displayedPairTimes.Add(pairTime);
                SubjectTimes.ItemsSource = _displayedPairTimes;
            }
        }

        private void InitTaskGrid(bool firstInit = true)
        {
            if (firstInit)
            {
                _displayedTaskTableItems = new List<TaskTableItem>();
                TasksGrid.ItemsSource = _displayedTaskTableItems;
            }

            _displayedTaskTableItems.Clear();
            if (CertainSubjectTasks.IsChecked != null && (bool) CertainSubjectTasks.IsChecked)
            {
                if (AwailableSubjects.SelectedIndex == -1 && AwailableSubjects.Items.Count > 0)
                {
                    AwailableSubjects.SelectedIndex = 0;
                    return;
                }
                Subject selectedSubject = AwailableSubjects.SelectedItem as Subject;

                if (selectedSubject == null)
                {
                    return;
                }
                foreach (var task in _taskRepository.GetAllTasksBySubjectId(selectedSubject.SubjectId))
                {
                    _displayedTaskTableItems.Add(new TaskTableItem()
                    {
                        TaskTableItemTask = task,
                        TaskTableItemSubject = _subjectRepository.GetSubjectById(task.SubjectId)
                    });
                }
                TasksGrid.Items.Refresh();
            }
            else
            {
                foreach (var task in _taskRepository.GetAllTasks())
                {
                    _displayedTaskTableItems.Add(new TaskTableItem()
                    {
                        TaskTableItemTask = task,
                        TaskTableItemSubject = _subjectRepository.GetSubjectById(task.SubjectId)
                    });
                }
                TasksGrid.Items.Refresh();
            }
        }

        private void CertainSubjectTasks_OnChecked(object sender, RoutedEventArgs e)
        {
            AwailableSubjects.IsEnabled = true;
            InitTaskGrid(false);

        }

        private void CertainSubjectTasks_OnUnchecked(object sender, RoutedEventArgs e)
        {
            AwailableSubjects.IsEnabled = false;
            InitTaskGrid(false);
        }

        private void AwailableSubjects_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InitTaskGrid(false);
        }

        private void TeachersList_Selected(object sender, RoutedEventArgs e)
        {
            //during deleting that method executes for some reason
            //thats why need to do that check

            if (TeachersList.Items.Count == 0)
            {
                return;
            }

            Teacher selectedTeacher = TeachersList.SelectedItem as Teacher;

            if (selectedTeacher == null)
            {
                return;
            }

            TeacherDescription.Text = "Teacher Description:\n" +
                                      ((selectedTeacher.TeacherDescription != string.Empty)
                                          ? selectedTeacher.TeacherDescription
                                          : "No description awailable");
            TeacherName.Text = "Teacher full name:\n" + selectedTeacher.FirstName + " " + selectedTeacher.MiddleName +
                               " " + selectedTeacher.LastName;

            var subjectList = _subjectRepository.GetTeacherSubjects(selectedTeacher);
            TeacherSubjects.Text = "Teacher Subjects:\n";
            if (subjectList.Count == 0)
            {
                TeacherSubjects.Text += "Teacher does not have subjects";
            }
            else
            {
                for (int i = 0; i < subjectList.Count; i++)
                {
                    if (i == 0)
                    {
                        TeacherSubjects.Text += subjectList[i].Name;
                        continue;
                    }
                    TeacherSubjects.Text += ", " + subjectList[i].Name;
                }
            }
        }

        private void TeachersTabSelected(object sender, RoutedEventArgs e)
        {
            // Automaticaly select first teacher from list when we click on "Teachers" tab
            // TODO
            // TeachersList.SelectedIndex = 0;
        }

        private void DeleteTeacher(object sender, ExecutedRoutedEventArgs e)
        {
            Teacher selectedTeacher = TeachersList.SelectedItem as Teacher;
            if (selectedTeacher == null)
            {
                return;
            }
            var result = MessageBox.Show("Are you sure you want delete teacher with name " + selectedTeacher.FirstName +
                                         selectedTeacher.MiddleName + " ?", "Confirm", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                return;
            }
            try
            {
                _teacherRepository.DeleteTeacherById(selectedTeacher.TeacherId);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Removing failed: " + ex.Message);
                return;
            }

            _displayedTeachers.Remove(selectedTeacher);
            TeachersList.Items.Refresh();
        }

        private void IsTeacherSelected(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = TeachersList.SelectedItem != null;
        }

        private void AddNewTeacher(object sender, RoutedEventArgs e)
        {
            var newTeacherWindow = new Dialogs.AddNewTeacherDialog();
            newTeacherWindow.ShowDialog();
            if (!newTeacherWindow.Selected)
            {
                return;
            }
            _displayedTeachers.Add(newTeacherWindow.AddedTeacher);
            TeachersList.Items.Refresh();

        }

        private void ModifyTeacher(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedTeacher = TeachersList.SelectedItem as Teacher;
            var modifyTeacherWindow = new Dialogs.ModifyTeacherDialog(selectedTeacher);
            modifyTeacherWindow.ShowDialog();
            if (!modifyTeacherWindow.Selected)
            {
                return;
            }
            _displayedTeachers[_displayedTeachers.IndexOf(selectedTeacher)] = modifyTeacherWindow.modifiedTeacher;
            TeachersList.Items.Refresh();
        }

        private void SubjectList_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (SubjectList.Items.Count == 0)
            {
                return;
            }

            Subject selectedSubject = SubjectList.SelectedItem as Subject;

            if (selectedSubject == null)
            {
                return;
            }

            SubjectDescription.Text = "Subject Description:\n" +
                                      ((selectedSubject.SubjectDescription != string.Empty)
                                          ? selectedSubject.SubjectDescription
                                          : "No description awailable");
        }

        private void IsSubjectSelected(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SubjectList.SelectedItem != null;
        }

        private void GetSubjectTeacher(object sender, ExecutedRoutedEventArgs e)
        {
            Subject selectedSubject = SubjectList.SelectedItem as Subject;
            if (selectedSubject == null)
            {
                return;
            }
            int teacherId = selectedSubject.TeacherId;
            TeachersList.SelectedItem =
                (from teacher in _displayedTeachers where teacher.TeacherId == teacherId select teacher).First();
            TeachersTab.IsSelected = true;
        }

        private void GetPreviousWeek(object sender, RoutedEventArgs e)
        {
            DateTime prevDate = _displayedDate - new TimeSpan(7, 0, 0, 0);
            if (prevDate < _displayedSemester.StartDate)
            {
                return;
            }
            try
            {
                _displayedSchedule = _timetableRepository.GetWeekByDate(prevDate);
            }
            catch (ArgumentException)
            {
                return;
            }
            SubjectControl.LoadSchedule(_displayedSchedule);
            WeekNumber.Text = _displayedSchedule.WeekNumber.ToString();
            RefreshWeekStats();
            _displayedDate = prevDate;
        }

        private void GetNextWeek(object sender, RoutedEventArgs e)
        {
            DateTime nextDate = _displayedDate + new TimeSpan(7, 0, 0, 0);
            if (nextDate>GetDayDateByDisplayedSemester(_displayedSemester.NumberOfWeeks,DaysOfWeek.Sunday))
            {
                return;
            }
            try
            {
                _displayedSchedule = _timetableRepository.GetWeekByDate(nextDate);
            }
            catch (ArgumentException)
            {
                return;
            }
            SubjectControl.LoadSchedule(_displayedSchedule);
            RefreshWeekStats();  
            _displayedDate = nextDate;
        }

        private void RefreshWeekStats()
        {
            WeekNumber.Text = _displayedSchedule.WeekNumber.ToString();
            WeekStartDate.Text = _displayedSchedule.ScheduleDayColumns.Count==0?"Schedule is empty":
                GetDayDateByDisplayedSemester(_displayedSchedule.WeekNumber,
                    _displayedSchedule.ScheduleDayColumns.Min(col => col.DayName)).ToString("dd/MM/yyyy");
            WeekEndDate.Text = _displayedSchedule.ScheduleDayColumns.Count == 0 ? "Schedule is empty" : GetDayDateByDisplayedSemester(_displayedSchedule.WeekNumber,
                    _displayedSchedule.ScheduleDayColumns.Max(col => col.DayName)).ToString("dd/MM/yyyy");
        }

        private void AddNewScheduleColumn(object sender, RoutedEventArgs e)
        {
            List<DaysOfWeek> freeDays = new List<DaysOfWeek>();
            foreach (DaysOfWeek item in Enum.GetValues(typeof(DaysOfWeek)))
            {
                freeDays.Add(item);
            }
            foreach (Entities.DayOfWeek day in _displayedSchedule.ScheduleDayColumns)
            {
                freeDays.Remove(day.DayName);
            }
            if (_displayedSchedule.WeekNumber == 1)
            {
                freeDays.RemoveAll(freeday =>
                {
                    DateTime dateDayTakePlace = GetDayDateByDisplayedSemester(_displayedSchedule.WeekNumber, freeday);
                    return dateDayTakePlace < _displayedSemester.StartDate;
                });
            }
            Dialogs.SelectDayDialog newDay = new Dialogs.SelectDayDialog(freeDays);
            newDay.ShowDialog();
            if (!newDay.IsSelected)
            {
                return;
            }
            DaysOfWeek dayToAdd = newDay.SelectedDay;
            _displayedSchedule.ScheduleDayColumns.Add(new Entities.DayOfWeek()
            {
                DayName = dayToAdd,
                SubjectDayDict = new Dictionary<int, SubjectScheduleItem>()
            });
            
            //Review DP: You should sort _displayedShedule before loading.
            SubjectControl.LoadSchedule(_displayedSchedule);
            RefreshWeekStats();
        }

        private DateTime GetDayDateByDisplayedSemester(int weekNumber, DaysOfWeek freeDay)
        {
            var weeksDifference = TimeSpan.FromTicks(new TimeSpan(7, 0, 0, 0).Ticks*(weekNumber - 1));
            var dayDifference =
                new TimeSpan(
                    (int) Enum.Parse(typeof(DaysOfWeek), _displayedSemester.StartDate.DayOfWeek.ToString()) -
                    (int) freeDay, 0, 0, 0);
            return _displayedSemester.StartDate + weeksDifference - dayDifference;
        }

        private void DeleteScheduleColumn(object sender, RoutedEventArgs e)
        {
            List<DaysOfWeek> freeDays = new List<DaysOfWeek>();
            foreach (var day in _displayedSchedule.ScheduleDayColumns)
            {
                freeDays.Add(day.DayName);
            }
            Dialogs.SelectDayDialog dialog = new Dialogs.SelectDayDialog(freeDays);
            dialog.ShowDialog();
            if (!dialog.IsSelected)
            {
                return;
            }
            DaysOfWeek dayToRemove = dialog.SelectedDay;
            _displayedSchedule.ScheduleDayColumns.RemoveAll(dayCol => dayCol.DayName == dayToRemove);
            SubjectControl.LoadSchedule(_displayedSchedule);
            _subjectRepository.RemoveAllTimeTableSubjectsFromDay(dayToRemove, _displayedSchedule.Year,
                _displayedSchedule.SemesterNumber, _displayedSchedule.WeekNumber);
            RefreshWeekStats();
        }

        private void RemoveSelectedTimeTableSubject(object sender, ExecutedRoutedEventArgs e)
        {
            DaysOfWeek selectedDay = SubjectControl.GetSelectedDay();
            int pairNumber = SubjectControl.GetSelectedPairNumber();
            foreach (var dayCol in _displayedSchedule.ScheduleDayColumns)
            {
                if (dayCol.DayName != selectedDay)
                {
                    continue;
                }
                dayCol.SubjectDayDict.Remove(pairNumber);
            }
            SubjectControl.LoadSchedule(_displayedSchedule);
            _subjectRepository.RemoveOneTimeTableSubject(_displayedSchedule.Year, _displayedSchedule.SemesterNumber,
                _displayedSchedule.WeekNumber, selectedDay, pairNumber);
        }

        private void IsTimeTableSubjectSelected(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SubjectControl.GetSelectedPairNumber() != -1;
        }

        private void IsTimeTableSubjectSelectedToRemove(object sender, CanExecuteRoutedEventArgs e)
        {
            int pairNumber = SubjectControl.GetSelectedPairNumber();
            if (pairNumber == -1)
            {
                return;
            }
            Entities.DayOfWeek selectedDay =
                _displayedSchedule.ScheduleDayColumns
                    .First(day => day.DayName == SubjectControl.GetSelectedDay());
            e.CanExecute = (pairNumber != -1) && (selectedDay.SubjectDayDict[pairNumber] != null);
        }

        private List<DaysOfWeek> GetDisplatedDays()
        {
            return (from day in _displayedSchedule.ScheduleDayColumns select day.DayName).ToList();
        }

        private void AddNewTimeTableSubject(object sender, RoutedEventArgs e)
        {
            var addDialog = new Dialogs.AddNewSubjectToTimeTableDialog(GetDisplatedDays());
            addDialog.ShowDialog();
            if (!addDialog.Selected)
            {
                return;
            }

            int pairNumber = -1;
            Subject selectedSubject = addDialog.SelectedSubject;
            DaysOfWeek selectedDay = addDialog.SelectedDay;
            PairType selectedPairType = addDialog.SelectedPairType;

            foreach (var day in _displayedSchedule.ScheduleDayColumns)
            {
                if (day.DayName == addDialog.SelectedDay)
                {
                    pairNumber = (day.SubjectDayDict.Count > 0) ? day.SubjectDayDict.Keys.Max() + 1 : 1;
                    day.SubjectDayDict.Add(pairNumber, new SubjectScheduleItem()
                    {
                        PairType = selectedPairType,
                        Subject = selectedSubject
                    });
                    break;
                }
            }
            SubjectControl.LoadSchedule(_displayedSchedule);
            _subjectRepository.AddModifyOneTimeTableSubject(_displayedSchedule.Year, _displayedSchedule.SemesterNumber,
                _displayedSchedule.WeekNumber, selectedDay, pairNumber, selectedSubject, selectedPairType);
        }

        private void ModifySelectedTimeTableSubject(object sender, ExecutedRoutedEventArgs e)
        {
            int selectedPairNumber = SubjectControl.GetSelectedPairNumber();
            DaysOfWeek selectedDay = SubjectControl.GetSelectedDay();

            SubjectScheduleItem selectedSubjectScheduleItem = SubjectControl.GetSelectedSubjectScheduleItem();
            var modifyDialog = new Dialogs.ModifyTimeTableSubjectDialog(_subjectRepository.GetAllSubjects(),
                selectedSubjectScheduleItem, _pairTypeRepository.GetAllPairTypes().ToList());
            modifyDialog.ShowDialog();

            if (!modifyDialog.IsSeleted && modifyDialog.SelectedPairType == null && modifyDialog.SelectedSubject == null)
            {
                return;
            }
            SubjectScheduleItem modifiedScheduleItem = new SubjectScheduleItem()
            {
                PairType = modifyDialog.SelectedPairType,
                Subject = modifyDialog.SelectedSubject
            };
            _displayedSchedule.ScheduleDayColumns.First(col => col.DayName == selectedDay).SubjectDayDict[
                selectedPairNumber] = modifiedScheduleItem;

            SubjectControl.LoadSchedule(_displayedSchedule);
            _subjectRepository.AddModifyOneTimeTableSubject(_displayedSchedule.Year, _displayedSchedule.SemesterNumber,
                _displayedSchedule.WeekNumber,
                selectedDay, selectedPairNumber, modifiedScheduleItem.Subject, modifiedScheduleItem.PairType);
        }

        private void IsTaskSelected(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = TasksGrid.SelectedIndex != -1;
        }

        private void DeleteTask(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedTask = (TaskTableItem) TasksGrid.SelectedItem;
            _displayedTaskTableItems.Remove(selectedTask);
            TasksGrid.Items.Refresh();
            _taskRepository.DeleteTaskById(selectedTask.TaskTableItemTask.TaskId);
        }

        private void ModifyTask(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedTask = (TaskTableItem) TasksGrid.SelectedItem;
            var modifyDialog = new Dialogs.AddModifyTaskDialog(selectedTask, _subjectRepository.GetAllSubjects());
            modifyDialog.ShowDialog();
            if (!modifyDialog.IsSelected)
            {
                return;
            }
            _taskRepository.ModifyTask(selectedTask.TaskTableItemTask.TaskId, modifyDialog.SelectedSubject.SubjectId,
                modifyDialog.SelectedDate, modifyDialog.SelectedTaskDescription,
                modifyDialog.SelectedTaskPriority);
            var targetTask =
                _displayedTaskTableItems.First(
                    task => task.TaskTableItemTask.TaskId == selectedTask.TaskTableItemTask.TaskId);

            targetTask.TaskTableItemSubject = modifyDialog.SelectedSubject;
            targetTask.TaskTableItemTask.DeadLineDate = modifyDialog.SelectedDate;
            targetTask.TaskTableItemTask.SubjectId = modifyDialog.SelectedSubject.SubjectId;
            targetTask.TaskTableItemTask.TaskDescription = modifyDialog.SelectedTaskDescription;
            targetTask.TaskTableItemTask.TaskPriority = modifyDialog.SelectedTaskPriority;

            TasksGrid.Items.Refresh();
        }

        private void TaskGrid_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (TasksGrid.SelectedIndex == -1)
            {
                return;
            }
            TaskDescription.Text = "Task description:\n" +
                                   ((TaskTableItem) TasksGrid.SelectedItem).TaskTableItemTask.TaskDescription;
        }

        private void AddNewTask(object sender, RoutedEventArgs e)
        {
            var addDialog = new Dialogs.AddModifyTaskDialog(_subjectRepository.GetAllSubjects());
            addDialog.ShowDialog();
            if (!addDialog.IsSelected)
            {
                return;
            }
            int insertedTaskId = _taskRepository.AddNewTask(addDialog.SelectedSubject.SubjectId,
                addDialog.SelectedTaskPriority,
                addDialog.SelectedDate,
                addDialog.SelectedTaskDescription);

            var newTask = new TaskTableItem()
            {
                TaskTableItemSubject = addDialog.SelectedSubject,
                TaskTableItemTask = new SubjectTask()
                {
                    DeadLineDate = addDialog.SelectedDate,
                    SubjectId = addDialog.SelectedSubject.SubjectId,
                    TaskPriority = addDialog.SelectedTaskPriority,
                    TaskDescription = addDialog.SelectedTaskDescription,
                    TaskId = insertedTaskId
                }
            };
            if ((CertainSubjectTasks.IsChecked != null
                 && (bool) CertainSubjectTasks.IsChecked
                 && ((Subject) AwailableSubjects.SelectedItem).SubjectId == newTask.TaskTableItemSubject.SubjectId)
                || (CertainSubjectTasks.IsChecked != null && !(bool) CertainSubjectTasks.IsChecked)
                || CertainSubjectTasks.IsChecked == null)
            {
                _displayedTaskTableItems.Add(newTask);
            }
            TasksGrid.Items.Refresh();
        }

        private void GetSubjectTasks(object sender, ExecutedRoutedEventArgs e)
        {
            Subject selectedSubject = SubjectList.SelectedItem as Subject;
            if (selectedSubject == null)
            {
                return;
            }
            if (CertainSubjectTasks.IsChecked != null && !(bool) CertainSubjectTasks.IsChecked)
            {
                CertainSubjectTasks.IsChecked = true;
            }
            AwailableSubjects.SelectedItem = selectedSubject;
            TasksTab.IsSelected = true;
        }

        private void DeleteSubject(object sender, ExecutedRoutedEventArgs e)
        {
            Subject selectedSubject = SubjectList.SelectedItem as Subject;
            if (selectedSubject == null)
            {
                return;
            }

            var result =
                MessageBox.Show(
                    "Are you sure you want delete subject \"" + selectedSubject.Name +
                    "\" ?\n All related tasks and TimeTable items will be deleted.", "Confirm", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                return;
            }

            _subjectRepository.DeleteSubjectById(selectedSubject.SubjectId);
            _displayedSubjects.Remove(selectedSubject);
            SubjectList.Items.Refresh();
            AwailableSubjects.Items.Refresh();
            InitTaskGrid(false);
            InitSubjectControl();
        }

        private void ModifySubject(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedSubject = SubjectList.SelectedItem as Subject;
            if (selectedSubject == null)
            {
                return;
            }
            var modifySubjectDialog = new Dialogs.AddModifySubjectDialog(selectedSubject,_teacherRepository.GetAllTeachers().ToList());
            modifySubjectDialog.ShowDialog();
            if (!modifySubjectDialog.IsSelected)
            {
                return;
            }
            _subjectRepository.ModifySubject(selectedSubject.SubjectId,
                                            modifySubjectDialog.SelectedSubjectName,
                                            modifySubjectDialog.SelectedSubjectDescription,
                                            modifySubjectDialog.SelectedTeacher.TeacherId);
            var targetSubject = _displayedSubjects.First(subj => subj.SubjectId == selectedSubject.SubjectId);
            targetSubject.Name = modifySubjectDialog.SelectedSubjectName;
            targetSubject.SubjectDescription = modifySubjectDialog.SelectedSubjectDescription;
            targetSubject.TeacherId = modifySubjectDialog.SelectedTeacher.TeacherId;
            AwailableSubjects.Items.Refresh();
            SubjectList.Items.Refresh();
        }

        private void AddNewSubject(object sender, RoutedEventArgs e)
        {
            var addDialog = new Dialogs.AddModifySubjectDialog(_teacherRepository.GetAllTeachers().ToList());
            addDialog.ShowDialog();
            if (!addDialog.IsSelected)
            {
                return;
            }

            int insertedId = -1;

            try
            {
                insertedId = _subjectRepository.AddNewSubject(addDialog.SelectedSubjectName, addDialog.SelectedTeacher.TeacherId, addDialog.SelectedSubjectDescription);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50005)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
            _displayedSubjects.Add(new Subject()
            {
                SubjectId = insertedId,
                SubjectDescription = addDialog.SelectedSubjectDescription,
                TeacherId = addDialog.SelectedTeacher.TeacherId,
                Name = addDialog.SelectedSubjectName
            });
            SubjectList.Items.Refresh();
            AwailableSubjects.Items.Refresh();
        }

        private void GetTimeTableSubjectInfo(object sender, ExecutedRoutedEventArgs e)
        {
            Subject selectedSubject = SubjectControl.GetSelectedSubjectScheduleItem().Subject;
            SubjectList.SelectedItem = _displayedSubjects.First(subj => subj.SubjectId == selectedSubject.SubjectId);
            SubjectsTab.IsSelected = true;
        }

        private void GetTimeTableTeacherInfo(object sender, ExecutedRoutedEventArgs e)
        {
            int selectedTeacherId = SubjectControl.GetSelectedSubjectScheduleItem().Subject.TeacherId;
            TeachersList.SelectedItem = _displayedTeachers.First(teacher => teacher.TeacherId == selectedTeacherId);
            TeachersTab.IsSelected = true;
        }

        private void GetTimeTableTasksInfo(object sender, ExecutedRoutedEventArgs e)
        {
            Subject selectedSubject = SubjectControl.GetSelectedSubjectScheduleItem().Subject;
            if (CertainSubjectTasks.IsChecked != null && !(bool)CertainSubjectTasks.IsChecked)
            {
                CertainSubjectTasks.IsChecked = true;
            }
            AwailableSubjects.SelectedItem = _displayedSubjects.First(subj => subj.SubjectId == selectedSubject.SubjectId);
            TasksTab.IsSelected = true;
        }

        private void CreateNewSemester(object sender, RoutedEventArgs e)
        {
            var addSemesterDialog = new Dialogs.AddSemesterDialog(_allSemesters);
            addSemesterDialog.ShowDialog();
            if (!addSemesterDialog.IsSelected)
            {
                return;
            }
            _semesterRepository.AddNewSemester(addSemesterDialog.SelectedSemesterYear,
                                              addSemesterDialog.SelectedSemesterNumber,
                                              addSemesterDialog.SelectedSemesterStartDate,
                                              addSemesterDialog.SelectedSemesterNumberOfWeeks,
                                              addSemesterDialog.SelectedStartPartOfWeek);
            _allSemesters.Add(new Semester()
            {
                YearValue = addSemesterDialog.SelectedSemesterYear,
                SemesterNumber = addSemesterDialog.SelectedSemesterNumber,
                StartDate = addSemesterDialog.SelectedSemesterStartDate,
                NumberOfWeeks = addSemesterDialog.SelectedSemesterNumberOfWeeks,
                StartPartOfWeek = addSemesterDialog.SelectedStartPartOfWeek
            });
            Semesters.Items.Refresh();
        }

        private void ChangeSemester(object sender, SelectionChangedEventArgs e)
        {
            var selectedSemester = Semesters.SelectedItem as Semester;
            if (selectedSemester == null 
                || (_displayedSemester!=null 
                && _displayedSemester.YearValue == selectedSemester.YearValue 
                && _displayedSemester.SemesterNumber == selectedSemester.SemesterNumber))
            {
                if (_displayedSchedule == null)
                {
                    InitSubjectControl();
                }
                return;
            }
            _displayedSemester = selectedSemester;
            InitSubjectControl();
        }

        private void DeleteSemester(object sender, RoutedEventArgs e)
        {
            if (Semesters.SelectedItem == null)
            {
                MessageBox.Show("Please select semester");
                return;
            }
            var result = MessageBox.Show("Are you sure you want delete current semester?\n That action will remove all related data ", "Confirm", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                return;
            }
            var semesterToDelete = Semesters.SelectedItem as Semester;
            _semesterRepository.DeleteSemester(semesterToDelete);

            _allSemesters.Remove(semesterToDelete);
            Semesters.Items.Refresh();

            if (_allSemesters.Count > 0)
            {
                Semesters.SelectedIndex = 0;
                return;
            }
            OutOfSemestersHandler();
        }

        private void IsTimeSelected(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = SubjectTimes.SelectedIndex != -1;
        }

        private void ChangeTime(object sender, ExecutedRoutedEventArgs e)
        {
            var selectedTime = SubjectTimes.SelectedItem as PairTime;
            if (selectedTime == null)
            {
                return;
            }
            var changePairTimeDialog = new Dialogs.ChangePairTimeDialog(selectedTime, _displayedPairTimes);
            changePairTimeDialog.ShowDialog();
            if (!changePairTimeDialog.IsSelected)
            {
                return;
            }
            var targetPair = _displayedPairTimes.First(time => time.PairNumber == selectedTime.PairNumber);
            targetPair.PairStart = changePairTimeDialog.SelectedPairStart;
            targetPair.PairEnd = changePairTimeDialog.SelectedPairEnd;
            _pairTimesRepository.AdjustPairTime(targetPair.PairNumber,targetPair.PairStart,targetPair.PairEnd);
            SubjectTimes.Items.Refresh();
        }
    }





}
