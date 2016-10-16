using System;
using System.Collections.Generic;
using System.Windows;
using StudentDiary.Entities;

namespace StudentDiary.UI.Dialogs
{
    /// <summary>
    /// Interaction logic for ChangePairTimeDialog.xaml
    /// </summary>
    public partial class ChangePairTimeDialog
    {
        public bool IsSelected { get; private set; }
        public TimeSpan SelectedPairStart { get; private set; }
        public TimeSpan SelectedPairEnd { get; private set; }
        private List<PairTime> ExistingPairTimes { get; }
        private PairTime OldTime { get; }

        public ChangePairTimeDialog(PairTime selectedTime, List<PairTime> displayedPairTimes)
        {
            InitializeComponent();
            ExistingPairTimes = displayedPairTimes;
            OldTime = selectedTime;
            StartTimePicker.Value = new DateTime() + selectedTime.PairStart;
            EndTimePicker.Value = new DateTime() + selectedTime.PairEnd;
        }

        private void ChangeTime(object sender, RoutedEventArgs e)
        {
            if (StartTimePicker.Value==null || EndTimePicker.Value==null)
            {
                MessageBox.Show("Please select time");
                return;
            }
            SelectedPairStart = TimeSpan.FromTicks(((DateTime)StartTimePicker.Value).Ticks);
            SelectedPairEnd = TimeSpan.FromTicks(((DateTime)EndTimePicker.Value).Ticks);
            if (ExistingPairTimes.Exists(time=>((time.PairNumber != OldTime.PairNumber) 
                                            &&(time.PairStart < SelectedPairStart) && (SelectedPairStart < time.PairEnd))
                                            || ((time.PairStart < SelectedPairEnd) && (SelectedPairEnd < time.PairEnd))))
            {
                MessageBox.Show("Can not add new times.\nPair times intersect");
                return;
            }

            IsSelected = true;
            Close();
        }
    }
}
