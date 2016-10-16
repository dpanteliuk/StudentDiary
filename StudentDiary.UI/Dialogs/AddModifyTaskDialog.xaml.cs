using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using StudentDiary.Entities;
using StudentDiary.UI.HelperClasses;

namespace StudentDiary.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for AddModifyTask.xaml
    /// </summary>
    public partial class AddModifyTaskDialog
    {
        public Subject SelectedSubject { private set; get; }
        public DateTime SelectedDate { private set; get; }
        public int SelectedTaskPriority { private set; get; }
        public string SelectedTaskDescription { private set; get; }
        public bool IsSelected { private set; get; }

        public AddModifyTaskDialog(List<Subject> subjectList)
        {
            InitializeComponent();
            AwailableSubjectsList.ItemsSource = subjectList;
            AwailableSubjectsList.SelectedIndex = 0;
        }

        public AddModifyTaskDialog(TaskTableItem selectedTask, List<Subject> subjectList)
        {
            InitializeComponent();
            AwailableSubjectsList.ItemsSource = subjectList;
            AwailableSubjectsList.SelectedItem = subjectList.First(subj => subj.SubjectId == selectedTask.TaskTableItemSubject.SubjectId);

            TaskDeadline.SelectedDate = selectedTask.TaskTableItemTask.DeadLineDate;
            TaskPriority.Text = selectedTask.TaskTableItemTask.TaskPriority.ToString();
            TaskDescription.Text = selectedTask.TaskTableItemTask.TaskDescription;
        }

        private void AddNewTask(object sender, RoutedEventArgs e)
        {
            int priority;
            SelectedSubject = AwailableSubjectsList.SelectedItem as Subject;

            if (TaskDeadline.SelectedDate == null)
            {
                MessageBox.Show("Deadline has to be picked");
                return;
            }

            SelectedDate = (DateTime)TaskDeadline.SelectedDate;

            if (SelectedDate < DateTime.Now)
            {
                MessageBox.Show("Task deadline can't be less then today's date");
                return;
            }

            if(!int.TryParse(TaskPriority.Text,out priority))
            {
                MessageBox.Show("Priority must be a number");
                return;
            }

            SelectedTaskPriority = priority;
            SelectedTaskDescription = TaskDescription.Text;
            IsSelected = true;
            Close();
        }
    }

}
