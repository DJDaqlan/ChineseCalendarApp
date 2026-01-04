using ChineseCalendar.Data;
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
using static System.Net.Mime.MediaTypeNames;

namespace ChineseCalendar.Views
{
    /// <summary>
    /// Interaction logic for ChineseCalendarWindow.xaml
    /// </summary>
    public partial class ChineseCalendarWindow : Window, WindowOperable
    {
        const int YEAR_RANGE = 100;
        CalendarService calendar;
        DataService dataService;
        DateConverterService dateConverter;
        CalendarDatabase database;
        String[] zodiacArray = [
            "Rat", "Ox", "Tiger", "Rabbit", 
            "Dragon", "Snake", "Horse", "Goat", 
            "Monkey", "Rooster", "Dog", "Boar"
            ];

        public ChineseCalendarWindow()
        {
            InitializeComponent();
            calendar = new CalendarService(CalendarService.calendarType.LunarChinese);
            dataService = new DataService("CalendarEvents.json", System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ChineseCalendar"));
            dateConverter = new DateConverterService();
            database = dataService.GetDatabase();
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

        public void LoadPage(Page newPage)
        {
            MainFrame.Navigate(newPage);
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
            for (int i = 0; i < calendar.WEEKDAY_COUNT; i++)
            {
                DaysGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            int increment = 0;
            if (daysNum % calendar.WEEKDAY_COUNT != 0) increment += 1;
            if (daysNum - (calendar.WEEKDAY_COUNT - (int)calendar.GetFirstDayOfMonth(year, month)) > calendar.WEEKDAY_COUNT * calendar.MONTH_WEEK_COUNT) increment += 1;
            int rows = daysNum / calendar.WEEKDAY_COUNT + increment;
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
            bool isShowCurrMonth = false;
            if (year.Equals(dateConverter.ToLunar(DateTime.Now).Year))
            {
                if (month.Equals(dateConverter.ToLunar(DateTime.Now).Month))
                {
                    isShowCurrMonth = true;
                }
            }

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

                LunarDate date;
                date = new LunarDate
                {
                    Year = year,
                    Month = month,
                    Day = d
                };
                // Checks for events on that specific day
                CalendarEvent calendarEvent = dataService.SearchEntry(database, date);

                bool cnyCheck = month == 1 && d == 1;

                // Format to signify today's date
                LunarDate todayLunar = dateConverter.ToLunar(DateTime.Today);
                if (isShowCurrMonth && d == todayLunar.Day)
                {
                    cell.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4A7C59"));
                    dayLabel.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
                }
                if (calendarEvent != null)
                {
                    cell.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C62828"));
                }

                Grid.SetRow(cell, row);
                Grid.SetColumn(cell, col);
                DaysGrid.Children.Add(cell);

                col += 1;
                if (col >= calendar.WEEKDAY_COUNT)
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
        private void UpdateMonthSelection(int year)
        {
            String[] monthRange = calendar.GetMonthRange(year);

            MonthComboBox.ItemsSource = monthRange;
            MonthComboBox.SelectedIndex = calendar.GetMonth() - 1;
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
            int displayedYear = calendar.GetYear();
            UpdateYearSelection(displayedYear);
            YearComboBox.SelectedItem = displayedYear;
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
            UpdateMonthSelection(int.Parse(YearComboBox.SelectedItem.ToString()));
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
            int desiredYear = int.Parse(YearComboBox.SelectedItem.ToString());
            int desiredMonth = 0;
            desiredMonth = (int)MonthComboBox.SelectedIndex + 1;

            calendar.SetMonth(desiredMonth);
            calendar.SetYear(desiredYear);

            LoadDate(calendar.GetYear(), calendar.GetMonth());
            LoadDateSelectors();
            LoadZodiac();
        }

        private void ViewEventsButton_Click(object sender, RoutedEventArgs e)
        {
            LoadPage(new ViewChineseEventPage(MainFrame));
        }

        private void MainFrame_NavigationStopped(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            LoadDate(calendar.GetYear(), calendar.GetMonth());
            LoadDateSelectors();
            database = dataService.GetDatabase();
        }
    }
}
