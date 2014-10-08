using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dumb_CHIP8
{
    public class CHIP8_GFX : Component
    {
        private dumb_CHIP8M _parent;
        private Byte[] GFX;
        private Boolean dirty;

        public CHIP8_GFX(dumb_CHIP8M parent)
        {
            _parent = parent;
        }
        public void init()
        {
            GFX = new Byte[64*32];
            dirty = true;
        }
        public void exec()
        {
            if (dirty)
                this.dirty = false;
            //TODO
        }
        public void stop()
        {
        }
        public void clear()
        {
            for (int i = 0; i < (64 * 32); i++)
                GFX[i] = 0x00;
            dirty = true;
        }
        public Boolean isDirty()
        {
            return dirty;
        }
        public Byte pixelAt(int x, int y)
        {
            return GFX[x + (64 * y)];//beware the stride
        }
        public void xorPixel(int x, int y)
        {
            if ((GFX[x + (64 * y)] ^= 1) != 0)//just xor the bit
                dirty = true;
        }
    }
}
