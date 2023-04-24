using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace mct_l3_v2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        private bool blickStatus = false;
       

        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            timer.Tick += new EventHandler(timerTick);
        }

        private void timerTick(object sender, EventArgs e) {
            if (blickStatus)
            {
                blickStatus = false;
                myArrow.Fill = new SolidColorBrush(Colors.Blue);
            } else {
                blickStatus = true;
                myArrow.Fill = new SolidColorBrush(Colors.Yellow);
            } 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // myArrow.Fill = new SolidColorBrush(Colors.Blue);
            timer.Start();
        }
    }
}
