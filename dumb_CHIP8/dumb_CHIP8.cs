﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dumb_CHIP8
{
    public class dumb_CHIP8M : Component
    {
        List <Component> parts = new List<Component>();
        List<dumb_Service> services = new List<dumb_Service>();

        public CHIP8_CPU _cpu;
        public CHIP8_RAM _ram;
        public CHIP8_GFX _gfx;
        public CHIP8_SND _snd;
        public CHIP8_KEY _key;

        public dumb_Input input;
        public dumb_Sound sound;
        public dumb_Video video;

        public dumb_CHIP8M()
        {
            this._cpu = new CHIP8_CPU(this);
            this._ram = new CHIP8_RAM(this);
            this._gfx = new CHIP8_GFX(this);
            this._snd = new CHIP8_SND(this); 
            this._key = new CHIP8_KEY(this);

            this.input = new dumb_Input(_key);
            this.sound = new dumb_Sound(_snd);
            this.video = new dumb_Video(_gfx);

            parts.Add(_cpu);
            parts.Add(_ram);
            parts.Add(_gfx);
            parts.Add(_snd);
            parts.Add(_key);

            services.Add(input);
            services.Add(sound);
            //services.Add(video);
        }
        public void init()
        {
            foreach(Component p in parts)
                p.init();
            foreach (dumb_Service s in services)
                s.init();
        }
        public void exec()
        {
            foreach (Component p in parts)
            {
                p.exec();
            }
        }
        public void stop()
        {
            foreach (Component p in parts)
            {
                p.stop();
            }
        }
    }
}
