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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalendarButton_Click(object sender, RoutedEventArgs e)
        {
            this.LoadWindow(new CalendarWindow());
        }

        public void LoadWindow(Window newWindow)
        {
            newWindow.Show();
            this.Close();
        }
    }
}
