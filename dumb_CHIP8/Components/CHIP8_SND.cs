using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dumb_CHIP8
{
    public class CHIP8_SND : Component
    {
        private dumb_CHIP8M _parent;
        private Boolean on;

        public CHIP8_SND(dumb_CHIP8M parent)
        {
            _parent = parent;
        }
        public void init()
        {
            on = false;
        }
        public void exec()
        {
            if (on)
                this.on = false;
            //TODO
        }
        public void stop()
        {
        }
        public void isNoisey()
        {
            on = true;
        }
    }
}
