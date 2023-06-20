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

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x407 dokumentiert.

namespace mct_l4_a1
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Ozilloskop oszi = new Ozilloskop();

        double[] values = new double[960];


        public MainPage()
        {
            this.InitializeComponent();
            oszi.neueDaten += Oszi_neueDaten;
        }

        private void Oszi_neueDaten(double[] buffer)
        {
            double a = -151.5151; // mirror over x-axis and scale in y direction
            double b = +250; // y offset

            PointCollection pts = new PointCollection();

            for (int i = 0; i < buffer.Length; i++)
            {
                pts.Add(new Point(i, a*buffer[i]+b));
            }


            double max = buffer.Max();
            double min = buffer.Min();
            double mean = buffer.Average();
            double eff = Math.Sqrt(buffer.Average(x => x * x));

            poly.Points = pts;

            Max.Text = String.Format("Max: {0:0.00} V", max);
            Min.Text = String.Format("Min: {0:0.00} V", min);
            Mean.Text = String.Format("Mean: {0:0.00} V", mean);
            Eff.Text = String.Format("Eff: {0:0.00} V", eff);

        }
        private void changedSlider(object sender, RangeBaseValueChangedEventArgs e)
        {
            oszi.TriggLvl = TrigLvl.Value;
        }
    }
}
