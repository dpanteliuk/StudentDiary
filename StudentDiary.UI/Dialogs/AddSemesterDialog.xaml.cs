using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using StudentDiary.Entities;

namespace StudentDiary.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for AddSemesterDialog.xaml
    /// </summary>
    public partial class AddSemesterDialog
    {
        private readonly List<Semester> _allSemesters;
        public bool IsSelected { get; private set; }

        public int SelectedSemesterNumberOfWeeks { get; private set; }
        public DateTime SelectedSemesterStartDate { get; private set; }
        public int SelectedSemesterNumber { get; private set; }
        public int SelectedSemesterYear { get; private set; }
        public TypeOfWeek SelectedStartPartOfWeek { get; private set; }

        public AddSemesterDialog(List<Semester> allSemesters)
        {
            InitializeComponent();
            _allSemesters = allSemesters;

            List<int> awailableYears =
            (from year in Enumerable.Range(DateTime.Now.Month < 9 ? DateTime.Now.Year : DateTime.Now.Year+1, 5)
                where (allSemesters.Count(sem => sem.YearValue == year) < 2)
                select year).ToList();

            if (awailableYears.Count==0)
            {
                MessageBox.Show("Message", "Unfortunately no free semesters awailable");
                Close();   
            }

            AwailableSemesterYears.ItemsSource = awailableYears;
            AwailableSemesterYears.SelectedIndex = 0;
            AwailableTypesOfWeeks.ItemsSource = Enum.GetValues(typeof(TypeOfWeek));
            AwailableTypesOfWeeks.SelectedIndex = 0;
        }

        private void RefreshDatapicker()
        {
            int startYear = (int)AwailableSemesterYears.SelectedItem;
                //(int)AwailableSemesterNumbers.SelectedValue == 2
                //? (int) AwailableSemesterYears.SelectedItem
                //: (int) AwailableSemesterYears.SelectedItem + 1;
            int endYear = startYear;
            int startMonth = (int)AwailableSemesterNumbers.SelectedValue == 1 ? 9 : 1;
            int endMonth = (int)AwailableSemesterNumbers.SelectedValue == 1 ? 12 : 8;

            SemesterStartDate.DisplayDateStart = new DateTime(startYear,startMonth,1);
            SemesterStartDate.DisplayDateEnd = new DateTime(endYear,endMonth,1);
            SemesterStartDate.SelectedDate = new DateTime(startYear, startMonth, 1);
        }

        private void RefreshSemesterNumberList()
        {
            var awailableSemesterNumbers = (from semNumber in Enumerable.Range(1, 2)
                                                  where !_allSemesters.Exists(sem => (sem.YearValue == (int)AwailableSemesterYears.SelectedValue) && sem.SemesterNumber == semNumber)
                                                  select semNumber).ToList();
            AwailableSemesterNumbers.ItemsSource = awailableSemesterNumbers;
            AwailableSemesterNumbers.Items.Refresh();
            AwailableSemesterNumbers.SelectedIndex = 0;
        }

        private void RefreshNumberOfWeeks()
        {
            int selectedYear = (int) AwailableSemesterYears.SelectedItem;
            if (SemesterStartDate.SelectedDate == null)
            {
                return;
            }
            DateTime selectedDate = (DateTime) SemesterStartDate.SelectedDate;

            int selectedSemesterNumber = (int) AwailableSemesterNumbers.SelectedItem;

            AwailableNumberOfWeeks.ItemsSource = Enumerable.Range(1,(int) (selectedSemesterNumber == 1
                ?(new DateTime(selectedYear + 1,1,1)- selectedDate).TotalDays / 7 
                : (new DateTime(selectedYear, 9, 1) - selectedDate).TotalDays / 7));
            AwailableNumberOfWeeks.SelectedIndex = 0;
        }

        private void YearSelected(object sender, SelectionChangedEventArgs e)
        {
            RefreshSemesterNumberList();
        }

        private void SemesterNumberSelected(object sender, SelectionChangedEventArgs e)
        {
            RefreshDatapicker();
        }

        private void DateSelected(object sender, SelectionChangedEventArgs e)
        {
            RefreshNumberOfWeeks();
        }

        private void CreateSemester(object sender, RoutedEventArgs e)
        {
            SelectedSemesterYear = (int)AwailableSemesterYears.SelectedItem;
            SelectedSemesterNumber = (int)AwailableSemesterNumbers.SelectedItem;
            if (SemesterStartDate.SelectedDate == null)
            {
                MessageBox.Show("Semester start date has to be picked");
                return;
            }
            SelectedSemesterStartDate = (DateTime) SemesterStartDate.SelectedDate;
            SelectedSemesterNumberOfWeeks = (int) AwailableNumberOfWeeks.SelectedItem;
            SelectedStartPartOfWeek = (TypeOfWeek)AwailableTypesOfWeeks.SelectedValue;
            IsSelected = true;
            Close();
        }
    }
}
