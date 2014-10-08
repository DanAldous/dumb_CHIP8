using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dumb_CHIP8
{
    public class CHIP8_RAM : Component
    {
        private dumb_CHIP8M _parent;
        private Byte[] RAM;

        public CHIP8_RAM(dumb_CHIP8M parent)
        {
            _parent = parent;
        }
        public void init()
        {
            RAM = new Byte[4096];
            loadFont();
        }
        public void exec()
        {
        }
        public void stop()
        {
        }
        private void loadFont()
        {
            Byte[] Font = new Byte[80] { 
                0xF0, 0x90, 0x90, 0x90, 0xF0, // 0
                0x20, 0x60, 0x20, 0x20, 0x70, // 1
                0xF0, 0x10, 0xF0, 0x80, 0xF0, // 2
                0xF0, 0x10, 0xF0, 0x10, 0xF0, // 3
                0x90, 0x90, 0xF0, 0x10, 0x10, // 4
                0xF0, 0x80, 0xF0, 0x10, 0xF0, // 5
                0xF0, 0x80, 0xF0, 0x90, 0xF0, // 6
                0xF0, 0x10, 0x20, 0x40, 0x40, // 7
                0xF0, 0x90, 0xF0, 0x90, 0xF0, // 8
                0xF0, 0x90, 0xF0, 0x10, 0xF0, // 9
                0xF0, 0x90, 0xF0, 0x90, 0x90, // A
                0xE0, 0x90, 0xE0, 0x90, 0xE0, // B
                0xF0, 0x80, 0x80, 0x80, 0xF0, // C
                0xE0, 0x90, 0x90, 0x90, 0xE0, // D
                0xF0, 0x80, 0xF0, 0x80, 0xF0, // E
                0xF0, 0x80, 0xF0, 0x80, 0x80  // F
            };
            for (int i = 0; i < 79; i++)
                RAM[0x050 + i] = Font[i];
        }
        public Byte readAt(int index)
        {
            return RAM[index];
        }
        public void writeAt(int index, Byte data)
        {
            RAM[index] = data;
        }
        public void loadFromROM( )
        {
            Byte[] theROM = new Byte[4096];
            UInt16 size = 0;

            Microsoft.Win32.OpenFileDialog OpenRom = new Microsoft.Win32.OpenFileDialog();
            OpenRom.FileName = "";
            OpenRom.DefaultExt = ".ch8";
            OpenRom.Filter = "CHIP8 programs (.ch8)|*.ch8";

            Nullable<bool> result = OpenRom.ShowDialog();

            if (result == true)
            {
                try
                {
                    System.IO.FileStream Input;
                    if ((Input = (System.IO.FileStream)OpenRom.OpenFile()) != null)
                    {
                        using (Input)
                        {
                            System.IO.MemoryStream Piper = new System.IO.MemoryStream(theROM);
                            while (Input.CanRead)
                            {
                                String currentLine = Input.ReadByte().ToString();
                                if (currentLine.CompareTo("-1") == 0)//aha, still reads the EOF char
                                    break;
                                Piper.WriteByte(Byte.Parse(currentLine));
                                size++;
                            }
                            theROM = Piper.ToArray();
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
                //theROM to RAM
                int i = 0;
                while (i <= size)
                {
                    RAM[0x200 + i] = theROM[i];
                    i++;
                }
            }
            else
                System.Windows.MessageBox.Show("Error: Could not open file.");
        }
    }
}
