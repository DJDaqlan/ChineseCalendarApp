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
using System.Text.Json;
using System.IO;

namespace ChineseCalendar.Views
{
    /// <summary>
    /// Interaction logic for AddEventPage.xaml
    /// </summary>
    public partial class AddChineseEventPage : Page
    {
        Frame hostFrame;
        String baseInput;
        const int YEAR_RANGE = 100;
        CalendarService calendar;
        DataService dataService;
        public AddChineseEventPage(Frame hostFrame)
        {
            InitializeComponent();
            this.hostFrame = hostFrame;
            calendar = new CalendarService(CalendarService.calendarType.LunarChinese);
            dataService = new DataService("CalendarEvents.json", System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ChineseCalendar"));
            UpdateYearSelection(DateTime.Now.Year);
            baseInput = NameTextBox.Text;
        }

        /// <summary>
        /// Updates/Initialises the year selection, with the suggested year as the centre.
        /// </summary>
        /// <param name="year">The median year.</param>
        private void UpdateYearSelection(int year)
        {
            YearComboBox.ItemsSource = calendar.GetYearRange(year, YEAR_RANGE);
            YearComboBox.SelectedItem = year;
        }

        /// <summary>
        /// Updates the month selection with the suggested range of choices. 
        /// Will set the selected choice to empty string to avoid logic errors with leap years
        /// </summary>
        /// <param name="choices">An array of potential month choices</param>
        private void UpdateMonthSelection(String[] choices)
        {
            MonthComboBox.ItemsSource = choices;
            MonthComboBox.SelectedItem = " ";
        }

        /// <summary>
        /// Updates the day selection with the suggested range of choices.
        /// Will set the selected choice to an empty string to avoid logic errors with months having 
        /// variable number of days.
        /// </summary>
        /// <param name="choices">An array of potential day choices</param>
        private void UpdateDaySelection(String[] choices)
        {
            DayComboBox.ItemsSource = choices;
            DayComboBox.SelectedIndex = DayComboBox.SelectedIndex;
        }

        /// <summary>
        /// Simple implementation ensures that all entries are not errors and give syntax errors. 
        /// Gives no implementation that the entries are correct, just if they are valid
        /// </summary>
        /// <param name="name">Input for event name</param>
        /// <param name="year">Input for event year</param>
        /// <param name="month">Input for event month</param>
        /// <param name="day">Input for event day</param>
        /// <returns>true of false determining if all inputs are valid</returns>
        private bool IsValidEntry(String name, int year, int month, int day)
        {
            if (name.Equals(baseInput))
            {
                MessageBox.Show("Please enter a name");
                return false;
            }
            if (year == null && !(YearCheckBox.IsChecked==true))
            {
                MessageBox.Show("Please enter a valid year");
                return false;
            }
            if (month == null && !(MonthCheckBox.IsChecked==true))
            {
                MessageBox.Show("Please enter a valid month");
                return false;
            }
            if (day == null && !(DayCheckBox.IsChecked==true))
            {
                MessageBox.Show("Please enter a valid day");
                return false;
            }
                return true;
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            String name = NameTextBox.Text;

            bool repeatYear = YearCheckBox.IsChecked == true;
            bool repeatMonth = MonthCheckBox.IsChecked == true;
            bool repeatDay = DayCheckBox.IsChecked == true;

            int year, month, day;
            year = repeatYear ? 0 : int.Parse(YearComboBox.SelectedItem.ToString());
            month = repeatMonth ? 0 : calendar.GetMonth((String)MonthComboBox.SelectedItem);
            day = repeatDay ? 0 : int.Parse(DayComboBox.SelectedItem.ToString());
            
            if (IsValidEntry(name, year, month, day))
            {
                dataService.AddEntry(new CalendarEvent
                {
                    Title = name,
                    Date = new LunarDate
                    {
                        Year = year,
                        Month = month,
                        Day = day
                    },
                    RepeatYear = repeatYear,
                    RepeatMonth = repeatMonth,
                    RepeatDay = repeatDay
                }, CalendarService.calendarType.LunarChinese); // Because this is the page to add a Chinese event linked to the Chinese Calendar
                SuccessPopup.IsOpen = true;
            }
        }

        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int year = int.Parse(YearComboBox.SelectedItem.ToString());
            String[] monthRange = calendar.GetMonthRange(year);
            UpdateMonthSelection(monthRange);
            UpdateDaySelection(calendar.GetDayRange(year, 1));
        }

        private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int year = int.Parse(YearComboBox.SelectedItem.ToString());
            int month = MonthComboBox.SelectedIndex;
            String[] days = calendar.GetDayRange(year, month+1);
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
        }

        private void MonthCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            MonthComboBox.IsEnabled = true;
        }

        private void DayCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            DayComboBox.IsEnabled = false;
        }

        private void DayCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DayComboBox.IsEnabled = true;
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SuccessPopup_Closed(object sender, EventArgs e)
        {
            hostFrame.Navigate(new ViewChineseEventPage(hostFrame));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            hostFrame.Navigate(new ViewChineseEventPage(hostFrame));
        }
    }
}
