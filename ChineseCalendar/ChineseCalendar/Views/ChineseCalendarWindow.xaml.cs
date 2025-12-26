using ChineseCalendar.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for ChineseCalendarWindow.xaml
    /// </summary>
    public partial class ChineseCalendarWindow : Window, WindowOperable
    {
        DateTime displayedDate;
        DateConverterService dateConverter = new DateConverterService();
        ChineseLunisolarCalendar calendar;
        public ChineseCalendarWindow()
        {
            InitializeComponent();
            calendar = new ChineseLunisolarCalendar();
            displayedDate = DateTime.Today;
            LoadDate(displayedDate);
        }

        public void LoadWindow(Window newWindow)
        {
            newWindow.Show();
            this.Close();
        }

        public void LoadDate(DateTime displayedDate)
        {
            int month = calendar.GetMonth(displayedDate);
            String engMonthName = dateConverter.MonthToString(month);
            String chineseMonthName = dateConverter.MonthToChinese(engMonthName);
            int leapMonth = calendar.GetLeapMonth(displayedDate.Year);
            if (month == leapMonth)
            {
                month -= 1;
                chineseMonthName = '闰' + chineseMonthName + " (Leap)";
            }
            else if (month > leapMonth)
            {
                month -= 1;
            }

            MonthLabel.Content = chineseMonthName;
            int year = calendar.GetYear(displayedDate);
            YearLabel.Content = year + "年";
            InitialiseDays(year, month);
            PopulateDays(year, month);
        }

        public void InitialiseDays(int year, int month)
        {
            int daysNum = calendar.GetDaysInMonth(year, month);
            DaysGrid.RowDefinitions.Clear();
            DaysGrid.ColumnDefinitions.Clear();
            for (int i = 0; i < 7; i++)
            {
                DaysGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            int rows = daysNum / 7 + Convert.ToInt32(daysNum % 7 != 0);
            for (int i = 0; i < rows; i++)
            {
                DaysGrid.RowDefinitions.Add(new RowDefinition());
            }
        }
        
        public void PopulateDays(int year, int month)
        {
            int startingDay = (int)calendar.GetDayOfWeek(displayedDate);
            int monthDays = calendar.GetDaysInMonth(year, month);

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
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            LoadWindow(new MainWindow());
        }

        private void GregorianCalendarButton_Click(object sender, RoutedEventArgs e)
        {
            LoadWindow(new CalendarWindow());
        }
    }
}
