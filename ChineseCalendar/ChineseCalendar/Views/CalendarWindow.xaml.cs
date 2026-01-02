using ChineseCalendar.Services;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChineseCalendar.Views
{
    /// <summary>
    /// Interaction logic for CalendarWindow.xaml
    /// </summary>
    public partial class CalendarWindow : Window, WindowOperable
    {
        const int YEAR_RANGE = 100;
        DateTime displayedDateTime;
        CalendarService calendar;
        public CalendarWindow()
        {
            InitializeComponent();
            calendar = new CalendarService(CalendarService.calendarType.Gregorian);
            displayedDateTime = DateTime.Now;
            LoadDate(displayedDateTime);
            LoadDateSelectors();
        }

        /// <summary>
        /// Loads the specific window of choice and closes this one.
        /// </summary>
        /// <param name="newWindow">The new window to open.</param>
        public void LoadWindow(Window newWindow)
        {
            newWindow.Show();
            this.Close();
        }

        /// <summary>
        /// Loads the window with informaiton about the selected date
        /// </summary>
        /// <param name="date"></param>
        private void LoadDate(DateTime date)
        {
            (int, int, int) dateNums = calendar.GetDateNum(date);
            (String, String, String) dateStrs = calendar.GetDateStr(date);
            MonthLabel.Content = dateStrs.Item2;
            YearLabel.Content = dateStrs.Item1;
            InitialiseDays(dateNums.Item1, dateNums.Item2);
            PopulateDays(dateNums.Item2, dateNums.Item1);
        }

        /// <summary>
        /// Initialises the window to present the date. Mainly divides the grid into cells for each individual day.
        /// </summary>
        /// <param name="year">The year to present.</param>
        /// <param name="month">The month to present.</param>
        private void InitialiseDays(int year, int month)
        {
            int daysNum = calendar.GetNumDays(year, month);

            DaysGrid.RowDefinitions.Clear();
            DaysGrid.ColumnDefinitions.Clear();
            for (int i = 0; i < 7; i++)
            {
                DaysGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            int increment = 0;
            if (calendar.GetFirstDayOfMonth(year, month).Equals(DayOfWeek.Saturday)) increment += 1;
            if (daysNum % 7 != 0) increment += 1;
            int rows = daysNum / 7 + increment;
            for (int i = 0; i < rows; i++)
            {
                DaysGrid.RowDefinitions.Add(new RowDefinition());
            }
        }

        /// <summary>
        /// Fills the calendar grid with the days of the month, allocated into the specific day of the week.
        /// Days of the week follow the Gregorian calendar.
        /// </summary>
        /// <param name="year">The year presented</param>
        /// <param name="month">The month presented</param>
        private void PopulateDays(int month, int year)
        {
            bool isShowCurrMonth = false;
            bool isLeapMonth = false;
            if (displayedDateTime.Year.Equals(DateTime.Now.Year))
            {
                if (displayedDateTime.Month.Equals(DateTime.Now.Month))
                {
                    isShowCurrMonth = true;
                }
            }
            if (displayedDateTime.Month == 2 && DateTime.IsLeapYear(year))
            {
                isLeapMonth = true;
            }
            int startingDay = (int)calendar.GetFirstDayOfMonth(year, month);
            int monthDays = calendar.GetNumDays(year, month);

            int row = 0;
            int col = startingDay;

            DaysGrid.Children.Clear();
            for (int d = 1; d <= monthDays; d++)
            {
                int day = d;
                if (isLeapMonth && d > 29)
                {
                    day = 29;
                }
                var dayLabel = new Label 
                {
                    Content = day.ToString(), 
                    HorizontalContentAlignment=HorizontalAlignment.Center, 
                    VerticalContentAlignment=VerticalAlignment.Center, 
                    HorizontalAlignment=HorizontalAlignment.Center, 
                    VerticalAlignment=VerticalAlignment.Center,
                    FontSize=18
                };

                var cell = new Border
                {
                    CornerRadius = new CornerRadius(6),
                    Margin = new Thickness(2),
                    Child = dayLabel
                };
                if (isShowCurrMonth && d.Equals(DateTime.Now.Day))
                {
                    cell.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DBEAFE"));
                }
                Grid.SetRow(cell, row);
                Grid.SetColumn(cell, col);
                DaysGrid.Children.Add(cell);

                col += 1;
                if (col >= 7)
                {
                    col = 0;
                    row += 1;
                }
            }
        }

        /// <summary>
        /// Initialises the year and month to today's date
        /// </summary>
        private void LoadDateSelectors()
        {
            int currentYear = DateTime.Now.Year;
            UpdateMonthSelections(currentYear);
            UpdateYearSelections(currentYear);
        }

        /// <summary>
        /// Updates the month range to be selected based on the current year that is selected.
        /// </summary>
        /// <param name="year">The year to change the month range to.</param>
        private void UpdateMonthSelections(int year)
        {
            MonthComboBox.ItemsSource = Enumerable.Range(1, 12).Select(m => new DateTime(year, m, 1).ToString("MMMM")).ToList();
            MonthComboBox.SelectedIndex = displayedDateTime.Month;
        }

        /// <summary>
        /// Updates the range of years to change to, with this year as the center.
        /// </summary>
        /// <param name="year"></param>
        private void UpdateYearSelections(int year)
        {
            YearComboBox.ItemsSource = Enumerable.Range(year - (YEAR_RANGE / 2), YEAR_RANGE + 1).ToList();
            YearComboBox.SelectedItem = displayedDateTime.Year;
        }

        private void PrevMonthButton_Click(object sender, RoutedEventArgs e)
        {
            int shownYear = displayedDateTime.Year;
            int shownMonth = displayedDateTime.Month;
            int newMonth = shownMonth - 1;
            int newYear = shownYear;
            if (newMonth <= 0)
            {
                newYear -= 1;
                newMonth = 12;
            }

            displayedDateTime = calendar.ChangeDate(newYear, newMonth);
            LoadDate(displayedDateTime);
        }

        private void NextMonthButton_Click(object sender, RoutedEventArgs e)
        {
            int shownYear = displayedDateTime.Year;
            int shownMonth = displayedDateTime.Month;
            int newMonth = shownMonth + 1;
            int newYear = shownYear;
            if (newMonth > 12)
            {
                newYear += 1;
                newMonth = 1;
            }

            displayedDateTime = calendar.ChangeDate(newYear, newMonth);
            LoadDate(displayedDateTime);
        }

        private void CurrMonthButton_Click(object sender, RoutedEventArgs e)
        {
            displayedDateTime = DateTime.Now;
            LoadDate(displayedDateTime);
        }

        private void SelectDateButton_Click(object sender, RoutedEventArgs e)
        {
            int desiredYear = int.Parse(YearComboBox.SelectedItem.ToString());
            int desiredMonth = MonthComboBox.SelectedIndex+1;

            displayedDateTime = calendar.ChangeDate(desiredYear, desiredMonth);
            LoadDate(displayedDateTime);
        }

        private void ChineseCalendarButton_Click(object sender, RoutedEventArgs e)
        {
            LoadWindow(new ChineseCalendarWindow());
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            LoadWindow(new MainWindow());
        }
    }
}
