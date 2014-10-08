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

namespace dumb_CHIP8
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static dumb_CHIP8M CHIP8;
        public static dumb_Video gfx;

        public MainWindow()
        {
            gfx = new dumb_Video();
            CHIP8 = new dumb_CHIP8M();
            //dumb_Video vid01 = new dumb_Video(ref CHIP8._gfx);   
            //vid01.Visibility = Visibility.Visible;
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
