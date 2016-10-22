using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using StudentDiary.Entities;
using StudentDiary.Repositories;

namespace StudentDiary.UI.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для AddNewTeacher.xaml
    /// </summary>
    
    //Review DP: I think it is appropriate to merge AddNewTeacherDialog and ModifyTeacherDialog in one.
    public partial class AddNewTeacherDialog
    {
        TeacherRepository teacherRepository = new TeacherRepository(ConfigurationManager.ConnectionStrings["StudentDiaryConnectionString"].ConnectionString);
        public Teacher AddedTeacher { private set; get; }
        public bool Selected { private set; get; }
        int _addedTeacherId;
        public AddNewTeacherDialog()
        {
            InitializeComponent();
        }
        private void AddNewTeacherForm(object sender, RoutedEventArgs e)
        {
            if (!AreRequiredFieldsFilled())
            {
                MessageBox.Show("Please fill at least First Name, Middle Name and Last Name","Required fields are not filled !");
                return;
            }
            try
            {
                _addedTeacherId=teacherRepository.AddNewTeacher(FirstName.Text, MiddleName.Text, LastName.Text, Description.Text);
            }
            catch(SqlException ex)
            {
                if (ex.ErrorCode == 50000)
                {
                    MessageBox.Show(ex.Message, "Teacher alredy exists!");
                }
                else MessageBox.Show(ex.Message, "Unexpected error!");
                return;
            }
            AddedTeacher = new Teacher { TeacherId=_addedTeacherId, FirstName = FirstName.Text, MiddleName = MiddleName.Text, LastName = LastName.Text, TeacherDescription = Description.Text };
            Selected = true;
            Close();
        }

        private bool AreRequiredFieldsFilled()
        {
            return FirstName.Text != string.Empty || MiddleName.Text != string.Empty || LastName.Text != string.Empty;
        }
    }
}
