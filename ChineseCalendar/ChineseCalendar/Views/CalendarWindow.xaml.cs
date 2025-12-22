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
        public CalendarWindow()
        {
            InitializeComponent();
        }

        private void ChineseCalendarButton_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow(new ChineseCalendarWindow());
            CloseWindow();
        }

        public void OpenWindow(Window newWindow)
        {
            newWindow.Show();
        }
        public void CloseWindow()
        {
            this.Close();
        }
    }
}
