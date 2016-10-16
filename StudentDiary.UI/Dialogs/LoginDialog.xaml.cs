using System.Configuration;
using System.Windows;
using StudentDiary.Entities;
using StudentDiary.Repositories;
using StudentDiary.UI.Code;

namespace StudentDiary.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for LoginDialog.xaml
    /// </summary>
    public partial class LoginDialog
    {
        public bool IsSelected { get; private set; }
        private readonly UserRepository _userRepository;
        public User LoginedUser { get; private set; }

        public LoginDialog()
        {
            InitializeComponent();
            _userRepository = new UserRepository(ConfigurationManager.ConnectionStrings["StudentDiaryConnectionString"].ConnectionString);
        }

        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void LoginUser(object sender, RoutedEventArgs e)
        {
            string writtenLogin = LoginInput.Text;
            string writtenPassword = PasswordInput.Password;
            LoginedUser = _userRepository.Login(writtenLogin, Encryptor.GetHash(writtenPassword));
            if (LoginedUser == null)
            {
                MessageBox.Show("User login or password is not correct", "Message");
                return;
            }
            IsSelected = true;
            Close();
        }
    }
}
