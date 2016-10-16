using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using StudentDiary.Entities;
using StudentDiary.Repositories;

namespace StudentDiary.UI.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для ModifyTeacher.xaml
    /// </summary>
    public partial class ModifyTeacherDialog : Window
    {
        public bool Selected {private set; get; }
        TeacherRepository teacherRepository;
        readonly int teacherId;
        public Teacher modifiedTeacher { private set; get; }
        public ModifyTeacherDialog(Teacher teacher)
        {
            InitializeComponent();
            teacherId = teacher.TeacherId;
            teacherRepository = new TeacherRepository(ConfigurationManager.ConnectionStrings["StudentDiaryConnectionString"].ConnectionString);
            ModifiedFirstName.Text = teacher.FirstName;
            ModifiedMiddleName.Text = teacher.MiddleName;
            ModifiedLastName.Text = teacher.LastName;
            ModifiedDescription.Text = teacher.TeacherDescription;
        }

        private void ModifyTeacherInfo(object sender, RoutedEventArgs e)
        {
            if (!AreRequiredFieldsFilled())
            {
                MessageBox.Show("Please fill at least First Name, Middle Name and Last Name", "Required fields are not filled !");
                return;
            }
            try
            {
                teacherRepository.ModifyTeacher(teacherId,ModifiedFirstName.Text, ModifiedMiddleName.Text, ModifiedLastName.Text, ModifiedDescription.Text);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Unexpected error!");
                return;
            }
            modifiedTeacher = new Teacher() { TeacherId = teacherId, FirstName = ModifiedFirstName.Text, MiddleName = ModifiedMiddleName.Text, LastName = ModifiedLastName.Text };
            Selected = true;
            Close();
        }

        private bool AreRequiredFieldsFilled()
        {
            return ModifiedFirstName.Text != string.Empty || ModifiedMiddleName.Text != string.Empty || ModifiedLastName.Text != string.Empty;
        }
    }
}
