using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dumb_CHIP8
{
    public class dumb_Debug : dumb_Service
    {
        private CHIP8_CPU _cpu;

        public dumb_Debug(ref CHIP8_CPU CPU)
        {
            _cpu = CPU;
        }
        public void init()
        {
        }
        public void update()
        {
            _cpu.regDump();
        }
        public void stop()
        {
        }
    }
}
