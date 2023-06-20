using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Support;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x407 dokumentiert.

namespace mct_l3_l1
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        IOPult ioPult = IOPult.GetCurrentPult();

        private DispatcherTimer timer = new DispatcherTimer();
        private bool status;
  
        public MainPage()
        {
            this.InitializeComponent();
            this.status = false;

            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, object e)
        {
            // copyIO();
            radios();
            setTexts();
        }

        private void radios() {
            byte portB = ioPult.PortB;

            if (Radio0.IsChecked.Value) {
                ioPult.PortA = (byte)(ioPult.PortB ^ 0xFF); ;
            }
            if (Radio1.IsChecked.Value)
            {
                ioPult.PortA = (byte)(ioPult.PortB & 0b_1111_0000);
            }
            if (Radio2.IsChecked.Value)
            {
                ioPult.PortA = (byte)(ioPult.PortB << 2);
            }
            if (Radio3.IsChecked.Value)
            {
                ioPult.PortA = (byte)(ioPult.PortB >> 6);
            }
        }

        private void Button_Start(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        private void Button_Stop(object sender, RoutedEventArgs e) { 
            timer.Stop();
        }

        public void copyIO(object sender, RoutedEventArgs e) {
            byte portB = ioPult.PortB;
            ioPult.PortA = ioPult.PortB;

            setTexts();
        }

        public void setTexts() {
            TextBox0.Text = "PortB: 0x" + ioPult.PortB.ToString("X");
            TextBox1.Text = "PortA: 0x" + ioPult.PortA.ToString("X");
        }
    }
}
