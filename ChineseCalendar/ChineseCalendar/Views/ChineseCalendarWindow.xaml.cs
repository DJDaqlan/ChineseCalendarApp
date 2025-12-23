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
        public ChineseCalendarWindow()
        {
            InitializeComponent();
            displayedDate = DateTime.Now;
            LoadDate(displayedDate);
        }

        public void LoadWindow(Window newWindow)
        {
            newWindow.Show();
            this.Close();
        }

        public void LoadDate(DateTime displayedDate)
        {
            ChineseLunisolarCalendar calendar = new ChineseLunisolarCalendar();
            int month = calendar.GetMonth(displayedDate);
            MonthLabel.Content = dateConverter.MonthToString(month);
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
