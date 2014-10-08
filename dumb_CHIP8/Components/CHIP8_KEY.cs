using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dumb_CHIP8
{
    public class CHIP8_KEY : Component
    {
        private dumb_CHIP8M _parent;
        private Byte[] KEY;
        private Boolean dirty;

        public CHIP8_KEY(dumb_CHIP8M parent)
        {
            _parent = parent;
        }
        public void init()
        {
            KEY = new Byte[16];
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
        public Byte nextKey()
        {
            return KEY[0];//dummy
        }
    }
}
