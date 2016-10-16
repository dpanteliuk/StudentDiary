using System.Collections.Generic;
using System.Windows;
using StudentDiary.Entities;

namespace StudentDiary.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for AddNewDayDialog.xaml
    /// </summary>
    public partial class SelectDayDialog : Window
    {
        public List<DaysOfWeek> FreeDays { get; private set; }
        public DaysOfWeek SelectedDay { get; private set; }
        public bool IsSelected { get; private set; }

        public SelectDayDialog(List<DaysOfWeek> freeDays)
        {
            InitializeComponent();
            FreeDays = freeDays;
            AwailableDayList.ItemsSource = freeDays;
            AwailableDayList.SelectedItem = freeDays[0];
        }

        private void AddNewDay(object sender, RoutedEventArgs e)
        {
            SelectedDay = (DaysOfWeek)AwailableDayList.SelectedItem;
            IsSelected = true;
            Close();
        }
    }
}
