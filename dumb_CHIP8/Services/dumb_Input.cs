using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace dumb_CHIP8
{
    public class dumb_Input : dumb_Service
    {
        private CHIP8_KEY _key;
        //private Keyboard kb;

        public dumb_Input(ref CHIP8_KEY KEY)
        {
            _key = KEY;
        }
        /*public void Dispose()
        {
        }*/
        public void init()
        {
        }
        public void update()
        {
        }
        public void stop()
        {
        }
    }
}
