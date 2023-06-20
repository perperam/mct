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
using System.Threading;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x407 dokumentiert.

namespace mct_l3_l1
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Blinker blinker;

        public MainPage()
        {
            this.InitializeComponent();

            blinker = new Blinker();
            blinker.links += Blinker_links;
            blinker.rechts += Blinker_rechts;
        }

        private void Blinker_links(bool anAus)
        {
            if (anAus)
            {
                bLinks.Fill = new SolidColorBrush(Windows.UI.Colors.Orange);
            }
            else { bLinks.Fill = new SolidColorBrush(Windows.UI.Colors.White); }
        }

        private void Blinker_rechts(bool anAus)
        {
            if (anAus)
            {
                bRechts.Fill = new SolidColorBrush(Windows.UI.Colors.Orange);
            }
            else { bRechts.Fill = new SolidColorBrush(Windows.UI.Colors.White); }
        }
    }
}
