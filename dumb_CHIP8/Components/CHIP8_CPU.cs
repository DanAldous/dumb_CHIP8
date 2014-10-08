using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dumb_CHIP8
{
    public class CHIP8_CPU : Component
    {
        private dumb_CHIP8M _parent;
    
        private Byte[] V;
        private UInt16 Opcode;
        private UInt16 Index;
        private UInt16 PC;
        private UInt16 delay_timer;
        private UInt16 sound_timer;
        private UInt16[] stack;
        private Byte sp;
        
        private Byte[] key;

        public CHIP8_CPU(dumb_CHIP8M parent)
        {
            _parent = parent;
        }
        public void init()
        {
            int i;

            V = new Byte[16];
            for (i = 0; i < 15; i++)
                V[i] = 0x00;
            
            stack = new UInt16[16];
            for (i = 0; i < 15; i++)
                stack[i] = 0x0000;
            sp = 0;
            
            key = new Byte[16];
            for (i = 0; i < 15; i++)
                key[i] = 0x00;
            
            PC = 0x200;//start address, 512 decimal
            delay_timer = 0;
            sound_timer = 0;
            Opcode = 0;
            Index = 0;
        }
        public void exec()
        {
            
            //fetch opcode
            Opcode = _parent._ram.readAt(PC);//first byte
            Opcode <<= 8;//push it over
            Opcode |= _parent._ram.readAt(PC+1); ;//or on the next
            //decode opcode

            UInt16 x = (UInt16)(Opcode & 0x0F00);
            x >>= 8;
            UInt16 y = (UInt16)(Opcode & 0x00F0);
            y >>= 4;
            UInt16 n = (UInt16)(Opcode & 0x000F);
            UInt16 nn = (UInt16)(Opcode & 0x00FF);
            UInt16 nnn = (UInt16)(Opcode & 0x0FFF);

            switch (Opcode & 0xF000)
            {
                case 0x0000:
                    {
                        switch (nn)
                        {
                            case 0x00E0://clear screen - CHECK
                                _parent._gfx.clear();
                                V[0xF] = 1;
                                PC += 2;
                                break;
                            case 0x00EE://return from sub - CHECK
                                PC = stack[--sp];
                                break;
                        }
                    }
                    break;
                case 0x1000://1NNN - jump to address NNN - CHECK
                    PC = nnn;
                    break;
                case 0x2000://2NNN - call subroutine at NNN - CHECK
                    stack[sp++] = PC;
                    PC = nnn;
                    break;
                case 0x3000://3XNN - skip next instruction if VX == NN - CHECK
                    {
                        PC += (ushort)((V[x] == (Byte)nn) ? 4 : 2);
                    }
                    break;
                case 0x4000://4XNN - skip next instruction if VX != NN - CHECK
                    {
                        PC += (ushort)((V[x] != (Byte)nn) ? 4 : 2);
                    }
                    break;
                case 0x5000://5XY0 - skip next instruction if VX == VY - CHECK
                    if (n == 0x0000)
                    {
                        PC += (ushort)(V[x] == V[y] ? 4 : 2);
                    }
                    break;
                case 0x6000://6XNN - set VX to NN - CHECK
                    {
                        V[x] = (Byte)nn;
                        PC += 2;
                    }

                    break;
                case 0x7000://7XNN - add NN to VX - CHECK
                    {
                        V[x] += (Byte)nn;
                        PC += 2;
                    }
                    break;
                case 0x8000://8XYN
                    {
                        switch (n)
                        {
                            case 0x0000://8XY0 - set VX to VY - CHECK
                                V[x] = V[y];
                                break;
                            case 0x0001://8XY1 - set VX to VX OR VY - CHECK
                                V[x] |= V[y];
                                break;
                            case 0x0002://8XY2 - set VX to VX AND VY - CHECK
                                V[x] &= V[y];
                                break;
                            case 0x0003://8XY3 - set VX to VX XOR VY - CHECK
                                V[x] ^= V[y];
                                break;
                            case 0x0004://8XY4 - add VY to VX, set VF to 1 if carry and 0 if not - CHECK
                                {
                                    V[0xF] = (Byte)((UInt16)V[x] + (UInt16)V[y] > 0x0100 ? 1 : 0);
                                    V[x] = (Byte)(V[x] + V[y]);
                                }
                                break;
                            case 0x0005://8XY5 - sub VY from VX, set VF to 0 if borrow and 1 if there isnt - CHECK
                                {
                                    V[0xF] = (Byte)((V[x] < V[y]) ? 0 : 1);
                                    V[x] = (Byte)(V[x] - V[y]);
                                }
                                break;
                            case 0x0006://8XY6 - shift VX right 1, VF set to LSB of VX befor shift - CHECK
                                {
                                    V[0xF] = (Byte)(V[x] & 0x01);
                                    V[x] >>= 1;
                                }
                                break;
                            case 0x0007://8XY7 - set VX to VY-VX, set VF to 0 if borrow and 1 if not - CHECK
                                {
                                    V[0xF] = (Byte)(V[x] > V[y] ? 0 : 1);
                                    V[x] = (Byte)(V[y] - V[x]);
                                }
                                break;
                            case 0x000E://8XYE - shift VX left 1, VF set to MSB of VX befor shift - CHECK
                                {
                                    V[0xF] = (Byte)(V[x] & 0xF0);
                                    V[x] <<= 1;
                                }
                                break;
                        }
                        PC += 2;
                        break;
                    }
                case 0x9000://9XY0 - skip next instruction if VX != VY - CHECK
                    if ((Opcode & 0xF00F) == 0x9000)
                    {
                        PC += (ushort)((V[x] != V[y]) ? 4 : 2);
                    }
                    break;
                case 0xA000://ANNN - set I to address NNN - CHECK
                    Index = nnn;
                    PC += 2;
                    break;
                case 0xB000://BNNN - jump to address NNN plus V0 - CHECK
                    PC = nnn;
                    PC += V[0];
                    break;
                case 0xC000://CXNN - set VX to rand AND NN - CHECK
                    {
                        Random ran = new System.Random();
                        Byte[] rand = new Byte[1];
                        ran.NextBytes(rand);
                        V[x] = (Byte)(rand[0] & nn);
                        PC += 2;
                    }
                    break;
                case 0xD000://DXYN - drawing eeek - CHECK
                    {
                        //Sprites stored in memory at location in index register (I), maximum 8bits wide. Wraps around the screen. 
                        //If when drawn, clears a pixel, register VF is set to 1 otherwise it is zero. All drawing is XOR drawing (e.g. it toggles the screen pixels)
                        ushort pixel;

                        V[0xF] = 0;
                        for (int i = 0; i < n; i++)
                        {
                            pixel = _parent._ram.readAt(Index + i);
                            for (int j = 0; j < 8; j++)
                                if ((pixel & (0x80 >> j)) != 0)
                                {
                                    if (_parent._gfx.pixelAt(x + j, y + i) == 1)
                                        V[0xF] = 1;
                                    _parent._gfx.xorPixel(x + j, y + i);
                                }
                        }
                    }
                    PC += 2;
                    break;
                case 0xE000://EX00 - key detection
                    {
                        switch (nn)
                        {
                            case 0x009E://EX9E - skip next instruction if key in Vx is pressed - CHECK
                                PC += (ushort)((key[V[x]] == 1) ? 4 : 2);
                                break;
                            case 0x00A1://EXA1 - skip next instruction if key in Vx isn't pressed - CHECK
                                PC += (ushort)((key[V[x]] == 0) ? 4 : 2);
                                break;
                        }
                    }
                    break;
                case 0xF000://Timers, Sprites, BCD oh my!
                    {
                        switch (nn)
                        {
                            case 0x0007://FX07 - set Vx to value of delay timer - CHECK
                                V[x] = (Byte)delay_timer;
                                break;
                            case 0x000A://FX0A - wait for key press and store in Vx
                                V[x] = _parent._key.nextKey();
                                break;
                            case 0x0015://FX15 - set delay timer to Vx - CHECK
                                delay_timer = V[x];
                                break;
                            case 0x0018://FX18 - set sound timer to Vx - CHECK
                                sound_timer = V[x];
                                break;
                            case 0x001E://FX1E - add Vx to Index - CHECK
                                Index += V[x];
                                break;
                            case 0x0029://FX29 - set Index to location of sprite for char in Vx - map index to mem location - CHECK
                                Index = (UInt16)(0x050 + (V[x] * 5));//base address 0x050 plus character value offset * 5 bytes per char
                                break;
                            case 0x0033://FX33 - store BCD rep of Vx at I - CHECK

                                _parent._ram.writeAt(Index,     (Byte)(V[x] / 100));
                                _parent._ram.writeAt(Index + 1, (Byte)((V[x] / 10) % 10));
                                _parent._ram.writeAt(Index + 2, (Byte)((V[x] % 100) % 10));
                                break;
                            case 0x0055://FX55 - store V0 through Vx in mem starting at Index - CHECK
                                for (int i = 0; i <= x; i++)
                                    _parent._ram.writeAt(Index + i, V[i]);
                                break;
                            case 0x0065://FX65 - store from Index to V0 through Vx - CHECK
                                for (int i = 0; i <= x; i++)
                                    V[i] = _parent._ram.readAt(Index + i);
                                break;
                        }
                        PC += 2;
                    }
                    break;
                default://might error this later
                    break;
            }
            //exec opcode

            //update timers
            if (delay_timer > 0)
                delay_timer--;
            if (sound_timer > 0)//also make a beep
            {
                _parent._snd.isNoisey();
                sound_timer--;
            }
        }
        public void stop()
        {
        }
        public void regDump()//debug
        {
            UInt16 x = (UInt16)(Opcode & 0x0F00);
            x >>= 8;
            UInt16 y = (UInt16)(Opcode & 0x00F0);
            y >>= 4;
            UInt16 nnn = (UInt16)(Opcode & 0x0FFF);

            System.Console.Write("Opcode: {0:X}\tStack[0]: {1:X}\t", Opcode, stack[0]);
            System.Console.Write("x: {0:X}\ty: {1:X}\tnnn: {2:X}\t", x, y, nnn);
            for (int i = 0; i < 16; i++)
                System.Console.Write("V[{0:X}]:\t{1:X}\t", i, V[i]);
            System.Console.Write("PC: {0:X}\tSP: {1:X}\tIndex: {2:X}\n", PC, sp, Index);
        }
    }
}