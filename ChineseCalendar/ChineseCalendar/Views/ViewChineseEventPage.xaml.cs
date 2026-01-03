using ChineseCalendar.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChineseCalendar.Views
{
    /// <summary>
    /// Interaction logic for ViewChineseEventPage.xaml
    /// </summary>
    public partial class ViewChineseEventPage : Page
    {
        Frame hostFrame;
        DataService dataService;
        DateConverterService dateConverter;
        public ViewChineseEventPage(Frame hostFrame)
        {
            InitializeComponent();
            this.hostFrame = hostFrame;
            dataService = new DataService("CalendarEvents.json", System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ChineseCalendar"));
            dateConverter = new DateConverterService();
            UpdateEvents();
        }

        /// <summary>
        /// Updates the list of events from the local database
        /// </summary>
        private void UpdateEvents()
        {
            InitialiseGrid();
            LoadData();
        }

        /// <summary>
        /// Initialises the grid so that the visuals are equally represented
        /// </summary>
        private void InitialiseGrid()
        {
            EventsListGrid.RowDefinitions.Clear();
            EventsListGrid.ColumnDefinitions.Clear();

            CalendarDatabase db = dataService.GetDatabase();
            for (int i = 0; i < db.Events.Count; i++)
            {
                RowDefinition rowDefine = new RowDefinition { Height=GridLength.Auto};
                EventsListGrid.RowDefinitions.Add(rowDefine);
            }
        }

        /// <summary>
        /// Loads the grid with the list of events
        /// </summary>
        private void LoadData()
        {
            int row = 0;
            EventsListGrid.Children.Clear();

            CalendarDatabase db = dataService.GetDatabase();
            for (int e = 0; e < db.Events.Count; e++)
            {
                CalendarEvent calendarEvent = db.Events[e];
                LunarDate nextOccurrence = dateConverter.GetNextOccurrence(calendarEvent.Date);
                DateTime gregEventDate = dateConverter.ToGregorian(nextOccurrence.Year, nextOccurrence.Month, nextOccurrence.Day);

                var eventCheckBox = new CheckBox
                {
                    Content = "Event: " + calendarEvent.Title.ToString() + 
                    ", Date: " + gregEventDate.Day + "/" + gregEventDate.Month + "/" + gregEventDate.Year + 
                    ", Repeating (Annually): " + (calendarEvent.RepeatYear ? "Yes" : "No") + " (Monthly): " + (calendarEvent.RepeatMonth ? "Yes" : "No") + " (Daily): " + (calendarEvent.RepeatDay ? "Yes" : "No"),
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    FontSize = 14
                };

                Grid.SetRow(eventCheckBox, row);
                EventsListGrid.Children.Add(eventCheckBox);
                row += 1;
            }
        }

        private void AddEventButton_Click(object sender, RoutedEventArgs e)
        {
            hostFrame.Navigate(new AddChineseEventPage(hostFrame));
        }

        private void RemoveEventButton_Click(object sender, RoutedEventArgs e)
        {
            UIElementCollection allEvents = EventsListGrid.Children;
            List<CalendarEvent> events = dataService.GetDatabase().Events;

            int index = 0;
            foreach (UIElement element in allEvents)
            {
                CheckBox elementCheckBox = (CheckBox) element;
                if (elementCheckBox.IsChecked == true)
                {
                    dataService.RemoveEntry(events[index].Title);
                }
                index++;
            }
            UpdateEvents();
        }
    }
}
