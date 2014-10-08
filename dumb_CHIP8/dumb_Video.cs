/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace dumb_CHIP8
{
    public class dumb_Video : dumb_Service
    {
        private WriteableBitmap GFX_Core;
        private CHIP8_GFX _gfx;

        public dumb_Video(CHIP8_GFX GFX)
        {
            _gfx = GFX;
            GFX_Core = new WriteableBitmap(64, 32, 120, 120d, PixelFormats.BlackWhite, BitmapPalettes.BlackAndWhite);
            graphics.Source = GFX_Core;
            graphics.Visibility = Visibility.Visible;
        }
        public void Dispose()
        {
        }
        public void init()
        {
            graphics.Source = GFX_Core;
            Int32Rect screenSize = new Int32Rect(0, 0, 64, 32);
            Byte[,] grid;
            grid = new Byte[64, 32];
            for (int i = 0; i < 64; i++)
                for (int j = 0; j < 32; j++)
                    grid[i, j] = (Byte)0x00;
            GFX_Core.WritePixels(screenSize, grid, 64, 0);
        }
        public void update()
        {
            Int32Rect screenSize = new Int32Rect(0, 0, 64, 32);
            Byte[] grid;
            grid = new Byte[64 * 32];//flat array for WritePixels is simpler
            for (int i = 0; i < 64; i++)
                for (int j = 0; j < 32; j++)
                    grid[i + (j * 64)] = (Byte)_gfx.pixelAt(i, j);
            GFX_Core.WritePixels(screenSize, grid, 64, 0);

            graphics.InvalidateVisual();
        }
    }
}*/