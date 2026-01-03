using ChineseCalendar.Data;
using ChineseCalendar.Services;
using ChineseCalendar.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace ChineseCalendar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, WindowOperable
    {
        DataService dataService;
        DateConverterService dateConverter;

        Brush baseCalendarButtonBackground;
        public MainWindow()
        {
            InitializeComponent();
            dataService = new DataService("CalendarEvents.json", System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ChineseCalendar"));
            dateConverter = new DateConverterService();

            baseCalendarButtonBackground = CalendarButton.Background;

            LoadEvent();
        }

        private void LoadEvent()
        {
            LunarDate chineseDate = dateConverter.ToLunar(DateTime.Today);
            CalendarEvent calendarEvent = dataService.SearchEntry(dataService.GetDatabase(), chineseDate);
            if (calendarEvent != null)
            {
                EventLabel.Content = calendarEvent.Title;
            }
        }

        private void CalendarButton_Click(object sender, RoutedEventArgs e)
        {
            this.LoadWindow(new CalendarWindow());
            //this.LoadWindow(new ChineseCalendarWindow());
        }

        public void LoadWindow(Window newWindow)
        {
            newWindow.Show();
            this.Close();
        }

        private void ChangeButtonColour(Button item, String hexcode)
        {
            Brush colour = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hexcode));
            item.Background = colour;
        }

        private void ChangeButtonColour(Button item, Brush colour)
        {
            item.Background = colour;
        }

        private void CalendarButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeButtonColour(CalendarButton, "#861818");
        }

        private void CalendarButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeButtonColour(CalendarButton, "#6F1212");
        }

        private void CalendarButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ChangeButtonColour(CalendarButton, baseCalendarButtonBackground);
        }

        private void CalendarButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangeButtonColour(CalendarButton, baseCalendarButtonBackground);
        }
    }
}
