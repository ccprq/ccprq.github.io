using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;

namespace ConsoleApplication3
{
    class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleMode(IntPtr handle, out int mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int handle);

        public static void SetChar(int x, int y, char txt, int r, int g, int b)
        {
            Console.SetCursorPosition(x, y);
            Console.Write("\x1b[38;2;" + r + ";" + g + ";" + b + "m" + txt);
            Console.ResetColor(); Console.CursorVisible = false;
        }
        public static void Write(int x_start, int x_end, int y_start, int y_end, int unit, char txt, int r, int g, int b)
        {
            for(int i = x_start; i < x_end;i++)
            {
                for(int j = y_start; j < y_end;j++)
                {
                    for (int h = 0; h < unit;h++)
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write("\x1b[38;2;" + r + ";" + g + ";" + b + "m" + txt);
                    }
                }
            }
            Console.ResetColor(); Console.CursorVisible = false;
        }
        public static void WriteVertical(string yon, int x, int y, int unit, char txt, int r, int g, int b)
        {
            if(yon == "ly")
            {
                for(int i = 0;i < unit;i++)
                {
                    Console.SetCursorPosition(x - i,y - i);
                    Console.Write("\x1b[38;2;" + r + ";" + g + ";" + b + "m" + txt);               
                }
            }
            else if (yon == "la")
            {
                for (int i = 0; i < unit; i++)
                {
                    Console.SetCursorPosition(x - i, y + i);
                    Console.Write("\x1b[38;2;" + r + ";" + g + ";" + b + "m" + txt);
                }
            }
            else if (yon == "ra")
            {
                for (int i = 0; i < unit; i++)
                {
                    Console.SetCursorPosition(x + i, y + i);
                    Console.Write("\x1b[38;2;" + r + ";" + g + ";" + b + "m" + txt);
                }
            }
            else if (yon == "ry")
            {
                for (int i = 0; i < unit; i++)
                {
                    Console.SetCursorPosition(x + i, y - i);
                    Console.Write("\x1b[38;2;" + r + ";" + g + ";" + b + "m" + txt);
                }
            } Console.CursorVisible = false;
        }
        static void Main(string[] args)
        {
            var handle = GetStdHandle(-11);
            int mode;
            GetConsoleMode(handle, out mode);
            SetConsoleMode(handle, mode | 0x4);
            Console.Title = "Patates sever menü 15TL";
            Console.OutputEncoding = UTF8Encoding.UTF8;         
            Write(10, 37, 15, 30, 10, '█',255,0,0);
            Write(18, 19, 20, 25, 5, '█', 255, 255, 0);
            WriteVertical("ra", 19, 20, 5, '█', 255, 255, 0);
            WriteVertical("ry", 23, 24, 5, '█', 255, 255, 0);
            Write(28, 29, 20, 25, 5, '█', 255, 255, 0);
            Write(12, 35, 15, 16, 10, '█', 0, 0, 0);
            Write(13, 34, 16, 17, 10, '█', 0, 0, 0);
            Write(13, 34, 16, 17, 10, '█', 139, 128, 0);
            SetChar(12, 16, '█', 139, 128, 0);
            SetChar(11, 15, '█', 139, 128, 0);
            SetChar(10, 15, '█', 139, 128, 0);
            SetChar(11, 15, '█', 139, 128, 0);
            SetChar(10, 15, '█', 139, 128, 0);
            SetChar(34, 16, '█', 139, 128, 0);
            SetChar(35, 15, '█', 139, 128, 0);
            SetChar(36, 15, '█', 139, 128, 0);
            Write(15, 17, 14, 16, 10, '█', 255,255,0);
            Write(18, 20, 11, 16, 10, '█', 255, 255, 0);
            Write(22, 24, 9, 16, 10, '█', 255, 255, 0);
            Write(26, 28, 14, 16, 10, '█', 255, 255, 0);
            Write(12, 14, 12, 16, 10, '█', 255, 255, 0);
            Write(30, 32, 14, 16, 10, '█', 255, 255, 0);
            Write(34, 36, 11, 15, 10, '█', 255, 255, 0);
            Write(22, 24, 12, 16, 10, '█', 255, 255, 0);
            Write(26, 28, 13, 16, 10, '█', 255, 255, 0);
            SetChar(34, 15, '█', 255, 255, 0);
            SetChar(34, 11, '█', 255, 255, 0);
            SetChar(23, 9, '█', 255, 255, 0);
            SetChar(22, 12, '█', 139, 128, 0);
            SetChar(18, 14, '█', 139, 128, 0);
            SetChar(13, 12, '█', 139, 128, 0);
            SetChar(34, 12, '█', 139, 128, 0);
            SetChar(30, 14, '█', 139, 128, 0);

            Write(11, 36, 30, 31, 10, '█', 255, 255, 0);
            Write(12, 35, 31, 32, 10, '█', 255, 0, 0);
            Console.CursorVisible = false;
            Console.ReadLine();
        }
    }
}
