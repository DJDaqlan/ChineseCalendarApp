using ChineseCalendar.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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
    /// Interaction logic for ChineseCalendarWindow.xaml
    /// </summary>
    public partial class ChineseCalendarWindow : Window, WindowOperable
    {
        const int YEAR_RANGE = 100;
        DateTime displayedDate;
        CalendarService calendar;
        String[] zodiacArray = [
            "Rat", "Ox", "Tiger", "Rabbit", 
            "Dragon", "Snake", "Horse", "Goat", 
            "Monkey", "Rooster", "Dog", "Boar"
            ];

        public ChineseCalendarWindow()
        {
            InitializeComponent();
            displayedDate = DateTime.Today;
            calendar = new CalendarService(CalendarService.calendarType.LunarChinese);
            LoadDate(calendar.GetYear(), calendar.GetMonth());
            LoadDateSelectors();
            LoadZodiac();

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

        public void LoadPage(String pagePath)
        {
            MainFrame.Source = new Uri(pagePath, UriKind.Relative);
        }

        /// <summary>
        /// Loads the window with information about the selected date
        /// </summary>
        /// <param name="year">The year</param>
        /// <param name="month">The month</param>
        private void LoadDate(int year, int month)
        {
            (String, String) dateStrs = calendar.GetDateStr(year, month);

            InitialiseDays(year, month);
            PopulateDays(year, month);

            MonthLabel.Content = dateStrs.Item2;
            YearLabel.Content = dateStrs.Item1;
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
        private void PopulateDays(int year, int month)
        {
            int startingDay = (int)calendar.GetFirstDayOfMonth(year, month);
            int monthDays = calendar.GetNumDays(year, month);

            int row = 0;
            int col = startingDay;

            DaysGrid.Children.Clear();
            for (int d = 1; d <= monthDays; d++)
            {
                var dayLabel = new Label
                {
                    Content = d.ToString(),
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 18
                };

                var cell = new Border
                {
                    CornerRadius = new CornerRadius(6),
                    Margin = new Thickness(2),
                    Child = dayLabel
                };

                // Format to signify CNY
                if (month == 1 && d == 1)
                {
                    cell.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C62828"));
                }

                // Format to signify today's date
                if (new ChineseLunisolarCalendar().ToDateTime(year, month, d, 0, 0, 0, 0).Equals(DateTime.Today))
                {
                    cell.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4A7C59"));
                    dayLabel.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
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
        /// Updates the month range to be selected based on the current year that is selected.
        /// </summary>
        /// <param name="year">The year to change the month range to.</param>
        private void UpdateMonthSelection(int year, int month)
        {
            String[] monthRange = calendar.GetMonthRange(year);

            MonthComboBox.ItemsSource = monthRange;
            calendar.SetMonth(month);
            // CAUTION: Has high coupling with MonthLabel. Suggested to find another method.
            MonthComboBox.SelectedIndex = calendar.GetMonth(MonthLabel.Content.ToString()) + 1;
        }
        
        /// <summary>
        /// Updates the range of years to change to, with this year as the center.
        /// </summary>
        /// <param name="year"></param>
        private void UpdateYearSelection(int year)
        {
            int[] yearRange = calendar.GetYearRange(year, YEAR_RANGE);
            YearComboBox.ItemsSource = yearRange;
        }
        
        /// <summary>
        /// Initialises the year and month to today's date
        /// </summary>
        private void LoadDateSelectors()
        {
            UpdateMonthSelection(displayedDate.Year, displayedDate.Month);
            int currentYear = DateTime.Today.Year;
            UpdateYearSelection(currentYear);
            YearComboBox.SelectedItem = displayedDate.Year;
        }

        /// <summary>
        /// Loads the specific zodiac animal for the year. All images were sourced from:
        /// https://www.ultimatekilimanjaro.com/chinese-zodiac-animals-what-they-are-and-what-they-mean/
        /// </summary>
        private void LoadZodiac()
        {
            int year = calendar.GetYear();
            int zodiacIndex = (year % 12) - 4;
            if (zodiacIndex < 0)
            {
                zodiacIndex = 12 + zodiacIndex;
            }
            Uri path = new Uri("pack://application:,,,/Assets/ChineseZodiacs/" + zodiacArray[zodiacIndex] + ".png", UriKind.Absolute);
            ImageSource image = new BitmapImage(path);

            ZodiacImage.ImageSource = image;
        }
        public void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateMonthSelection((int)YearComboBox.SelectedItem, MonthComboBox.SelectedIndex);
        }
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            LoadWindow(new MainWindow());
        }
        private void GregorianCalendarButton_Click(object sender, RoutedEventArgs e)
        {
            LoadWindow(new CalendarWindow());
        }
        private void DateSelectButton_Click(object sender, RoutedEventArgs e)
        {
            int desiredYear = (int)YearComboBox.SelectedItem;
            int desiredMonth = 0;
            desiredMonth = (int)MonthComboBox.SelectedIndex + 1;

            LoadDate(desiredYear, desiredMonth);
            
        }

        private void AddEventButton_Click(object sender, RoutedEventArgs e)
        {
            LoadPage("AddChineseEventPage.xaml");
        }
    }
}
