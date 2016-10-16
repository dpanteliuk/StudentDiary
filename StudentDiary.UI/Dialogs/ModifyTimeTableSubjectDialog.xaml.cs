using System.Collections.Generic;
using System.Linq;
using System.Windows;
using StudentDiary.Entities;

namespace StudentDiary.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for ModifyTimeTableDialog.xaml
    /// </summary>
    public partial class ModifyTimeTableSubjectDialog
    {
        public PairType SelectedPairType { get; private set; }
        public Subject SelectedSubject { get; private set; }
        public bool IsSeleted { get; private set; }

        public ModifyTimeTableSubjectDialog(List<Subject> awailableSubjects, SubjectScheduleItem selectedSubjectScheduleItem, List<PairType> awailablePairTypes)
        {
            InitializeComponent();
            AwailableSubjectsList.ItemsSource = awailableSubjects;
            AwailableSubjectsList.SelectedItem = selectedSubjectScheduleItem != null
                ? awailableSubjects.First(subject => subject.SubjectId == selectedSubjectScheduleItem.Subject.SubjectId)
                : awailableSubjects[0];

            AwailablePairTypesList.ItemsSource = awailablePairTypes;
            AwailablePairTypesList.SelectedItem = selectedSubjectScheduleItem != null
                ? awailablePairTypes.First(p => p.TypeId == selectedSubjectScheduleItem.PairType.TypeId)
                : awailablePairTypes[0]; 
            
        }

        private void ModifySubject(object sender, RoutedEventArgs e)
        {
            SelectedPairType = AwailablePairTypesList.SelectedItem as PairType;
            SelectedSubject = AwailableSubjectsList.SelectedItem as Subject;

            IsSeleted = true;
            Close();
        }
    }
}
