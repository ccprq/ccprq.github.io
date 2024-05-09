using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApplication3
{
    class Program
    {
        public static void SetChar(int x, int y, char txt, ConsoleColor fore)
        {
            Console.ForegroundColor = fore;
            Console.SetCursorPosition(x, y);
            Console.Write(txt);
            Console.ResetColor(); Console.CursorVisible = false;
        }
        public static void Write(int x_start, int x_end, int y_start, int y_end, int unit, char txt, ConsoleColor fore)
        {
            Console.ForegroundColor = fore;
            for(int i = x_start; i < x_end;i++)
            {
                for(int j = y_start; j < y_end;j++)
                {
                    for (int h = 0; h < unit;h++)
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write(txt);
                    }
                }
            }
            Console.ResetColor(); Console.CursorVisible = false;
        }
        public static void WriteVertical(string yon, int x, int y, int unit, char txt, ConsoleColor fore)
        {
            Console.ForegroundColor = fore;
            if(yon == "ly")
            {
                for(int i = 0;i < unit;i++)
                {
                    Console.SetCursorPosition(x - i,y - i);
                    Console.Write(txt);               
                }
            }
            else if (yon == "la")
            {
                for (int i = 0; i < unit; i++)
                {
                    Console.SetCursorPosition(x - i, y + i);
                    Console.Write(txt);
                }
            }
            else if (yon == "ra")
            {
                for (int i = 0; i < unit; i++)
                {
                    Console.SetCursorPosition(x + i, y + i);
                    Console.Write(txt);
                }
            }
            else if (yon == "ry")
            {
                for (int i = 0; i < unit; i++)
                {
                    Console.SetCursorPosition(x + i, y - i);
                    Console.Write(txt);
                }
            } Console.CursorVisible = false;
        }
        static void Main(string[] args)
        {         
            Console.Title = "Patates sever menü 15TL";
            Console.OutputEncoding = UTF8Encoding.UTF8;         
            Write(10, 37, 15, 30, 10, '█', ConsoleColor.Red);
            Write(18, 19, 20, 25, 5, '█', ConsoleColor.DarkYellow);
            WriteVertical("ra", 19, 20, 5, '█', ConsoleColor.DarkYellow);
            WriteVertical("ry", 23, 24, 5, '█', ConsoleColor.DarkYellow);
            Write(28, 29, 20, 25, 5, '█', ConsoleColor.DarkYellow);
            Write(12, 35, 15, 16, 10, '█', ConsoleColor.Black);
            Write(13, 34, 16, 17, 10, '█', ConsoleColor.Black);
            Write(13, 34, 16, 17, 10, '█', ConsoleColor.DarkYellow);
            SetChar(12, 16, '█', ConsoleColor.DarkYellow);
            SetChar(11, 15, '█', ConsoleColor.DarkYellow);
            SetChar(10, 15, '█', ConsoleColor.DarkYellow);
            SetChar(11, 15, '█', ConsoleColor.DarkYellow);
            SetChar(10, 15, '█', ConsoleColor.DarkYellow);
            SetChar(34, 16, '█', ConsoleColor.DarkYellow);
            SetChar(35, 15, '█', ConsoleColor.DarkYellow);
            SetChar(36, 15, '█', ConsoleColor.DarkYellow);
            Write(15, 17, 14, 16, 10, '█', ConsoleColor.Yellow);
            Write(18, 20, 11, 16, 10, '█', ConsoleColor.Yellow);
            Write(22, 24, 9, 16, 10, '█', ConsoleColor.Yellow);
            Write(26, 28, 14, 16, 10, '█', ConsoleColor.Yellow);
            Write(12, 14, 12, 16, 10, '█', ConsoleColor.Yellow);
            Write(30, 32, 14, 16, 10, '█', ConsoleColor.Yellow);
            Write(34, 36, 11, 15, 10, '█', ConsoleColor.Yellow);
            Write(22, 24, 12, 16, 10, '█', ConsoleColor.Yellow);
            Write(26, 28, 13, 16, 10, '█', ConsoleColor.Yellow);
            SetChar(34, 15, '█', ConsoleColor.Yellow);
            SetChar(34, 11, '█', ConsoleColor.Black);
            SetChar(23, 9, '█', ConsoleColor.Black);
            SetChar(22, 12, '█', ConsoleColor.DarkYellow);
            SetChar(18, 14, '█', ConsoleColor.DarkYellow);
            SetChar(13, 12, '█', ConsoleColor.DarkYellow);
            SetChar(34, 12, '█', ConsoleColor.DarkYellow);
            SetChar(30, 14, '█', ConsoleColor.DarkYellow);

            Write(11, 36, 30, 31, 10, '█', ConsoleColor.Red);
            Write(12, 35, 31, 32, 10, '█', ConsoleColor.Red);

            SetChar(35, 31, '█', ConsoleColor.DarkGray);
            SetChar(36, 30, '█', ConsoleColor.DarkGray);
            Write(37, 38, 15, 30, 9, '█', ConsoleColor.DarkGray);
            Console.CursorVisible = false;
            Console.ReadLine();
        }
    }
}
