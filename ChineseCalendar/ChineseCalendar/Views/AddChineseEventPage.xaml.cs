using ChineseCalendar.Data;
using ChineseCalendar.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChineseCalendar.Views
{
    /// <summary>
    /// Interaction logic for AddEventPage.xaml
    /// </summary>
    public partial class AddChineseEventPage : Page
    {
        const int YEAR_RANGE = 100;
        CalendarService calendar;
        public AddChineseEventPage()
        {
            InitializeComponent();
            calendar = new CalendarService(CalendarService.calendarType.LunarChinese);
            UpdateYearSelection(DateTime.Now.Year);
        }
        private void UpdateYearSelection(int year)
        {
            YearComboBox.ItemsSource = calendar.GetYearRange(year, YEAR_RANGE);
            YearComboBox.SelectedItem = year;
        }

        private void UpdateMonthSelection(String[] choices)
        {
            MonthComboBox.ItemsSource = choices;
            MonthComboBox.SelectedItem = " ";
        }

        private void UpdateDaySelection(String[] choices)
        {
            DayComboBox.ItemsSource = choices;
            DayComboBox.SelectedIndex = DayComboBox.SelectedIndex;
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            CalendarEvent newEvent = new CalendarEvent();
            if (true)
            {
            }
            
        }

        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int year = (int)YearComboBox.SelectedItem;
            String[] monthRange = calendar.GetMonthRange(year);
            UpdateMonthSelection(monthRange);

        }

        private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int year = (int)YearComboBox.SelectedItem;
            int month = MonthComboBox.SelectedIndex;
            String[] days = calendar.GetDayRange(year, month);
            UpdateDaySelection(days);
        }

        private void YearCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            YearComboBox.IsEnabled = false;
        }

        private void YearCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            YearComboBox.IsEnabled = true;
        }

        private void MonthCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MonthComboBox.IsEnabled = false;
            if (YearComboBox.IsEnabled) YearComboBox.IsEnabled = false;
        }

        private void MonthCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MonthComboBox.IsEnabled = true;
            if (!(bool)YearCheckBox.IsChecked) YearComboBox.IsEnabled = true;
        }

        private void DayCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            DayComboBox.IsEnabled = false;
            if (MonthComboBox.IsEnabled) MonthComboBox.IsEnabled = false;
            if (YearComboBox.IsEnabled) YearComboBox.IsEnabled = false;
        }

        private void DayCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DayComboBox.IsEnabled = true;
            if (!(bool)MonthCheckBox.IsChecked) MonthComboBox.IsEnabled = true;
            if (!(bool)YearCheckBox.IsChecked) YearComboBox.IsEnabled = true;
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void HidePopupButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
