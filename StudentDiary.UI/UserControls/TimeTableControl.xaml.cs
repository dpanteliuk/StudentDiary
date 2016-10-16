using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using StudentDiary.Entities;

namespace StudentDiary.UI.UserControls
{
    /// <summary>
    /// Логика взаимодействия для WeekUserControl.xaml
    /// </summary>
    public partial class TimeTableControl
    {
        public WeekSchedule WeekSchedule { get; private set; }
        private List<Dictionary<int, SubjectScheduleItem>> ListOfDays { get; set; }
        private ObservableCollection<ListBox> DayListBoxes { set; get; }
        public SelectionChangedEventHandler DayClickHandler { set; get; }

        public TimeTableControl()
        {
            InitializeComponent();
        }
        public void LoadSchedule(WeekSchedule weekSchedule)
        {
            Week.Children.Clear();
            WeekSchedule = weekSchedule;
            ListOfDays = new List<Dictionary<int, SubjectScheduleItem>>();
            DayListBoxes = new ObservableCollection<ListBox>();

            foreach (Entities.DayOfWeek day in WeekSchedule.ScheduleDayColumns)
            {
                var dayList = new ListBox
                {
                    Margin = new Thickness(5),
                    MinHeight = 164,
                    Background = Brushes.AliceBlue,
                    SelectionMode = SelectionMode.Single
                };
                Dictionary<int, SubjectScheduleItem> daySubjects = day.SubjectDayDict;
                if (day.SubjectDayDict.Keys.Count != 0)
                {
                    for (int i = 1; i <= daySubjects.Keys.Max(); i++)
                    {
                        if (!daySubjects.ContainsKey(i))
                        {
                            daySubjects.Add(i, null);
                        }
                    }
                }

                dayList.SelectionChanged += DayClick;
                if (DayClickHandler != null)
                {
                    dayList.SelectionChanged += DayClickHandler;
                }
                // Look of the listbox element
                dayList.ItemTemplate = (DataTemplate) FindResource("DayListTemplate");
                dayList.ItemsSource = daySubjects;
                // Sorting by subject number
                dayList.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Key",
                    System.ComponentModel.ListSortDirection.Ascending));

                // For displaying day name
                StackPanel dayWrapper = new StackPanel {Orientation = Orientation.Vertical, Width = 139};
                Label dayNameLabel = new Label {Content = day.DayName, HorizontalAlignment = HorizontalAlignment.Center};

                // Storing day name
                dayList.Tag = day.DayName;

                dayWrapper.Children.Add(dayNameLabel);
                dayWrapper.Children.Add(dayList);
                DayListBoxes.Add(dayList);

                ListOfDays.Add(daySubjects);
                Week.Children.Add(dayWrapper);

            }
        }

        public DaysOfWeek GetSelectedDay()
        {
            foreach (var day in DayListBoxes)
            {
                if (day.SelectedIndex != -1)
                {
                    return (DaysOfWeek) day.Tag;
                }
            }
            throw new Exception("No subject is selected");
        }

        public int GetSelectedPairNumber()
        {
            foreach (var day in DayListBoxes)
            {
                if (day.SelectedIndex != -1)
                {
                    return day.SelectedIndex + 1;
                }
            }
            return -1;
        }

        public SubjectScheduleItem GetSelectedSubjectScheduleItem()
        {
            foreach (var day in DayListBoxes)
            {
                if (day.SelectedIndex != -1)
                {
                    return ((KeyValuePair<int, SubjectScheduleItem>) day.SelectedItem).Value;
                }
            }
            return null;
        }

        private void DayClick(object sender, SelectionChangedEventArgs e)
        {
            var targetDay = (ListBox) e.OriginalSource;
            if (e.RemovedItems.Count != 0) return;
            foreach (var day in DayListBoxes)
            {
                if (Equals(day, targetDay))
                {
                    continue;
                }
                day.SelectedIndex = -1;
            }
        }
    }
}
