using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dumb_CHIP8
{
    public class dumb_Sound : dumb_Service
    {
        private CHIP8_SND _snd;
        
        public dumb_Sound(ref CHIP8_SND SND)
        {
            _snd = SND;
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
