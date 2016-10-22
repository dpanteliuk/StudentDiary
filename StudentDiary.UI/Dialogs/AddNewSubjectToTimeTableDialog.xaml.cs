using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using StudentDiary.Entities;
using StudentDiary.Repositories;

namespace StudentDiary.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for AddNewSubjectToTimeTable.xaml
    /// </summary>
    public partial class AddNewSubjectToTimeTableDialog
    {
        private readonly SubjectRepository _subjectRepository;
        private readonly PairTypeRepository _pairTypeRepository;

        public Subject SelectedSubject { get; private set; }
        public PairType SelectedPairType { get; private set; }
        public DaysOfWeek SelectedDay { get; private set; }
        public bool Selected { get; private set; }

        public AddNewSubjectToTimeTableDialog(List<DaysOfWeek> freeDays)
        {
            InitializeComponent();
            var connString = ConfigurationManager.ConnectionStrings["StudentDiaryConnectionString"].ConnectionString;
            _subjectRepository = new SubjectRepository(connString);
            _pairTypeRepository = new PairTypeRepository(connString);

            AwailableDaysList.ItemsSource = freeDays;
            AwailableDaysList.SelectedItem = freeDays[0];

            var awailableSubjects = _subjectRepository.GetAllSubjects();
            AwailableSubjectsList.ItemsSource = awailableSubjects;
            AwailableSubjectsList.SelectedItem = awailableSubjects[0];

            var awailablePairTypes = _pairTypeRepository.GetAllPairTypes().ToList();
            
            //Review DP: Your should add "Free" option in AwailablePairTypesList
            AwailablePairTypesList.ItemsSource = awailablePairTypes;
            AwailablePairTypesList.SelectedItem = awailablePairTypes[0];

        }

        private void AddNewSubject(object sender, RoutedEventArgs e)
        {
            SelectedSubject = (Subject)AwailableSubjectsList.SelectedItem;
            SelectedPairType = (PairType)AwailablePairTypesList.SelectedItem;
            SelectedDay = (DaysOfWeek)AwailableDaysList.SelectedItem;
            Selected = true;
            Close();
        }
    }
}
