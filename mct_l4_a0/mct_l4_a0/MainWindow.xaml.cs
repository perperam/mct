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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace mct_l4_a0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void bDreieck_Click(object sender, RoutedEventArgs e)
        {
            PointCollection pts = new PointCollection();
            pts.Add(new Point(0, 0));
            pts.Add(new Point(200, 0));
            pts.Add(new Point(100, 200));
            pts.Add(new Point(0, 0));
            poly.Points = pts;
        }

        private void bQuadrat_Click(object sender, RoutedEventArgs e)
        {
            PointCollection pts = new PointCollection();
            pts.Add(new Point(0, 0));
            pts.Add(new Point(200, 0));
            pts.Add(new Point(200, 200));
            pts.Add(new Point(0, 200));
            pts.Add(new Point(0, 0));
            poly.Points = pts;
        }

        private void bHaus_Click(object sender, RoutedEventArgs e)
        {
            PointCollection pts = new PointCollection();
            pts.Add(new Point(0, 200));
            pts.Add(new Point(0, 100));
            pts.Add(new Point(100, 100));
            pts.Add(new Point(100, 200));
            pts.Add(new Point(0, 200));
            pts.Add(new Point(100, 100));
            pts.Add(new Point(50, 50));
            pts.Add(new Point(0, 100));
            pts.Add(new Point(100, 200));
            poly.Points = pts;
        }

        private void bSinus_Click(object sender, RoutedEventArgs e)
        {
            PointCollection pts = new PointCollection();

            int T = 200;
            int A = 100;
            int O = 100;

            for(int x = 0; x <= T; x++)
            {
                pts.Add(new Point(x, - A * Math.Sin((2 * Math.PI / T) * x) + O));
            }
            poly.Points = pts;
        }
    }
}
