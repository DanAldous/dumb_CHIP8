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
    /// Interaction logic for dumb_Video.xaml
    /// </summary>
    public partial class dumb_Video : Page, dumb_Service
    {
        private WriteableBitmap GFX_Core;
        private CHIP8_GFX _gfx;
        private Int32Rect screenSize;
        private PixelFormat format;
        private BitmapPalette palette;

        public dumb_Video()
        {
            InitializeComponent();

            //_gfx = MainWindow.CHIP8._gfx;
            //MainWindow.gfx = this;
            screenSize = new Int32Rect(0, 0, 64, 32);
            format = PixelFormats.BlackWhite;
            palette = BitmapPalettes.BlackAndWhite;
            GFX_Core = new WriteableBitmap(64, 32, 120, 120d, format, palette);
            graphics.Source = GFX_Core;
            graphics.Visibility = Visibility.Visible;
        }
        public void BindGFX(CHIP8_GFX gfx)
        {
            _gfx=gfx;
        }
        public void init()
        {
            graphics.Source = GFX_Core;
            graphics.Width = 640;
            graphics.Height = 480;
            graphics.Stretch = Stretch.Uniform;
            int[] grid;
            grid = new int[64 * 32];
            for (int i = 0; i < 64; i++)
                for (int j = 0; j < 32; j++)
                    grid[i + (j * 64)] = (int)Colors.Black.GetHashCode();
            GFX_Core.WritePixels(screenSize, grid, 64, 0);
            graphics.InvalidateVisual();
            this.InvalidateVisual();
            this.Rect.Visibility = Visibility.Visible;
        }
        public void update()
        {
            if (_gfx.isDirty())
            {
                int[] grid;
                grid = new int[64 * 32];//flat array for WritePixels is simpler
                for (int i = 0; i < 64; i++)
                    for (int j = 0; j < 32; j++)
                        grid[i + (j * 64)] = _gfx.pixelAt(i, j);
                GFX_Core.WritePixels(screenSize, grid, 64, 0);
                graphics.InvalidateVisual();
                this.InvalidateVisual();
            }
        }
        public void stop()
        {
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            this.Start.Visibility = Visibility.Hidden;

            MainWindow.CHIP8.init();
            MainWindow.CHIP8._ram.loadFromROM();
            //vid01.Visibility = Visibility.Visible;
            //vid01.InvalidateVisual();
            //for(;;)
            for (int i = 0; i < 8192; i++)
                MainWindow.CHIP8.exec();
            this.Start.Visibility = Visibility.Visible;
        }
    }
}