using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Provider;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x407 dokumentiert.

namespace RGB_LED
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        LEDStreifen leds = new LEDStreifen();
        static byte[] datenSlider = new byte[24];

        DispatcherTimer timer = new DispatcherTimer();

        static int dir = 1;
        static int i = 0;
        public MainPage()
        {
            this.InitializeComponent();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            timer.Tick += Timer_Tick;
            timer.Start();
            
        }
        private void Timer_Tick(object sender, object e)
        {
            // daten = new byte[24];

            // set all null
            /*
            for (int i = 0; i < 24; i++)
            {
                daten[i] = 0x00;
            }
            */

            // walking forward
            if (dir == 1)
            {
                i++;
                if (i == 8) { dir = -1; }
                else
                {
                    datenSlider[i * 3] = 0xFF;
                }
            }
            // walking backwards
            else
            {
                i--;
                if (i == -1) { dir = 1; }
                else
                {
                    datenSlider[i * 3] = 0xFF;
                }
            }

            leds.sende(datenSlider);
        }

        private void Button_Reset(object sender, RoutedEventArgs e)
        {
            // this.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0xFF, 0xFF, 0xFF, 0x00));
            leds.reset();
        }

        private void Button_Rot(object sender, RoutedEventArgs e)
        {
            byte[] daten=new byte[24];

            for (int i = 1; i < 24; i+=3)
            {
                daten[i] = 0xFF;
            }

            leds.sende(daten);
        }

        private void Button_Gruen(object sender, RoutedEventArgs e)
        {
            byte[] daten = new byte[24];

            for (int i = 0; i < 24; i += 3)
            {
                daten[i] = 0xFF;
            }

            leds.sende(daten);
        }

        private void Button_Blau(object sender, RoutedEventArgs e)
        {
            byte[] daten = new byte[24];

            for (int i = 2; i < 24; i+=3)
            {
                daten[i] = 0xFF;
            }

            leds.sende(daten);
        }

        private void Button_Weiss(object sender, RoutedEventArgs e)
        {
            byte[] daten = new byte[24];

            for (int i = 0; i < 24; i++)
            {
                daten[i] = 0xFF;
            }

            leds.sende(daten);
        }

        private void Slider_Rot(object sender, RoutedEventArgs e)
        {
            for (int i = 1; i < 24; i+=3)
            {
                datenSlider[i] = (byte) SliderRot.Value;
            }
            ChangeRec();
            leds.sende(datenSlider);
        }

        private void Slider_Gruen(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 24; i+=3)
            {
                datenSlider[i] = (byte) SliderGruen.Value;
            }
            ChangeRec();
            leds.sende(datenSlider);
        }

        private void Slider_Blau(object sender, RoutedEventArgs e)
        {
            for (int i = 2; i < 24; i+=3)
            {
                datenSlider[i] = (byte) SliderBlau.Value;
            }
            ChangeRec();
            leds.sende(datenSlider);
        }

        private void ChangeRec()
        {
            DisplayRect.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(0xFF, (byte)SliderRot.Value, (byte)SliderGruen.Value, (byte)SliderBlau.Value));
        }

    }
}
