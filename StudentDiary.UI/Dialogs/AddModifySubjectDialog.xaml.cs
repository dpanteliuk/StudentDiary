using System.Collections.Generic;
using System.Linq;
using System.Windows;
using StudentDiary.Entities;

namespace StudentDiary.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for AddModifySubject.xaml
    /// </summary>
    public partial class AddModifySubjectDialog
    {
        public Teacher SelectedTeacher { private set; get; }
        public string SelectedSubjectName { private set; get; }
        public string SelectedSubjectDescription { private set; get; }
        public bool IsSelected { private set; get; }

        public AddModifySubjectDialog(List<Teacher> teachersList)
        {
            InitializeComponent();
            AwailableTeachersList.ItemsSource = teachersList;
            AwailableTeachersList.SelectedIndex = 0;
        }

        public AddModifySubjectDialog(Subject selectedSubject, List<Teacher> teacherList)
        {
            InitializeComponent();
            AwailableTeachersList.ItemsSource = teacherList;
            AwailableTeachersList.SelectedItem =
                teacherList.First(teacher => teacher.TeacherId == selectedSubject.TeacherId);

            SubjectName.Text = selectedSubject.Name;
            SubjectDescription.Text = selectedSubject.SubjectDescription;
        }

        private void AddNewSubject(object sender, RoutedEventArgs e)
        {
            SelectedTeacher = AwailableTeachersList.SelectedItem as Teacher;
            SelectedSubjectName = SubjectName.Text;
            SelectedSubjectDescription = SubjectDescription.Text;

            if (SelectedTeacher == null)
            {
                MessageBox.Show("Subject teacher has to be picked");
                return;
            }

            if (SelectedSubjectName == "")
            {
                MessageBox.Show("Subject name can not be empty");
                return;
            }

            IsSelected = true;
            Close();
        }
    }
}
