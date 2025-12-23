using ChineseCalendar.Services;
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
        DateConverterService dateConverter = new DateConverterService();
        DateTime displayedDateTime;
        public CalendarWindow()
        {
            InitializeComponent();
            displayedDateTime = DateTime.Now;
            LoadDate(displayedDateTime);
            LoadDateSelectors();
        }
        private void LoadDate(DateTime date)
        {
            MonthLabel.Content = dateConverter.MonthToString(date.Month);
            YearLabel.Content = date.Year;
            InitialiseDays();
            PopulateDays(date.Month, date.Year);
        }

        private void InitialiseDays()
        {
            DaysGrid.RowDefinitions.Clear();
            DaysGrid.ColumnDefinitions.Clear();
            for (int i = 0; i < 7; i++)
            {
                DaysGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < 6; i++)
            {
                DaysGrid.RowDefinitions.Add(new RowDefinition());
            }
        }

        private void PopulateDays(int month, int year)
        {
            bool isShowCurrMonth = false;
            if (displayedDateTime.Year.Equals(DateTime.Now.Year))
            {
                if (displayedDateTime.Month.Equals(DateTime.Now.Month))
                {
                    isShowCurrMonth = true;
                }
            }
            int startingDay = (int)(new DateTime(year, month, 1).DayOfWeek);
            int monthDays = DateTime.DaysInMonth(year, month);

            int row = 0;
            int col = startingDay;

            DaysGrid.Children.Clear();
            for (int d = 1; d <= monthDays; d++)
            {
                var dayLabel = new Label {
                    Content = d.ToString(), 
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

        private void LoadDateSelectors()
        {
            MonthComboBox.ItemsSource = Enumerable.Range(1, 12).Select(m => new DateTime(2025, m, 1).ToString("MMMM")).ToList();
            int currentYear = DateTime.Now.Year;
            YearComboBox.ItemsSource = Enumerable.Range(currentYear - (YEAR_RANGE / 2), YEAR_RANGE + 1).ToList();

            MonthComboBox.SelectedIndex = displayedDateTime.Month - 1;
            YearComboBox.SelectedItem = displayedDateTime.Year;
        }
        public void OpenWindow(Window newWindow)
        {
            newWindow.Show();
        }
        public void CloseWindow()
        {
            this.Close();
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

            DateTime newDate = new DateTime(newYear, newMonth, 1);
            displayedDateTime = newDate;
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

            DateTime newDate = new DateTime(newYear, newMonth, 1);
            displayedDateTime = newDate;
            LoadDate(displayedDateTime);
        }

        private void CurrMonthButton_Click(object sender, RoutedEventArgs e)
        {
            displayedDateTime = DateTime.Now;
            LoadDate(displayedDateTime);
        }

        private void SelectDateButton_Click(object sender, RoutedEventArgs e)
        {
            int desiredYear = (int)YearComboBox.SelectedItem;
            int desiredMonth = MonthComboBox.SelectedIndex+1;

            displayedDateTime = new DateTime(desiredYear, desiredMonth, 1);
            LoadDate(displayedDateTime);
        }
    }
}
