using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Threading.Tasks;
using static Terminal.TerminalColor;

namespace Terminal
{
    public static class Menu
    {
        public static void Selection<t>(t[] options, string forergb = "0,0,0", string backrgb = "255,255,255")
        {
            string r = "", g = "", b = "";
            int virgul = 0;
            for(int i = 0; i < Text.LengthOf(forergb);i++)
            {
                if(virgul ==  0 && Text.MakeString(forergb[i]) != ",")
                {
                    r += Text.MakeString(forergb[i]);
                    if (Text.MakeString(forergb[i]) == ",")
                    {
                        virgul++;
                    }
                }                
                else if(virgul == 1 && Text.MakeString(forergb[i]) != ",")
                {
                    g += Text.MakeString(forergb[i]);
                    if (Text.MakeString(forergb[i]) == ",")
                    {
                        virgul++;
                    }
                }
                else if (virgul == 2 && Text.MakeString(forergb[i]) != ",")
                {
                    b += Text.MakeString(forergb[i]);
                    if (Text.MakeString(forergb[i]) == ",")
                    {
                        virgul++;
                    }
                }
            }
            Text.Write(r+g+b);
        }
    }
    public static class Graph
    {
        [DllImport("User32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("User32.dll")]
        private static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);
        public static class Text
        {
            public static void DrawString(string text,string font, int fontsize, FontStyle style, Brush color, int x, int y)
            {
                IntPtr desktopPtr = GetDC(IntPtr.Zero);
                Graphics g = Graphics.FromHdc(desktopPtr);
                Font fontt = new Font(text,fontsize,style);
                g.DrawString(text, fontt, color, new PointF(x, y));
            }           
        }
        public static class Box
        {
            public static void DrawBox(int x, int y, int width, int height,Color color, bool argb = false,int alpha = 10, string forergb = "0,0,0")
            {
                IntPtr desktopPtr = GetDC(IntPtr.Zero);
                Graphics g = Graphics.FromHdc(desktopPtr);
                if(argb)
                {
                    SolidBrush b = new SolidBrush(System.Drawing.Color.FromArgb(alpha,TerminalColor.RGB.SolveRGB(forergb,'r'), TerminalColor.RGB.SolveRGB(forergb, 'g'), TerminalColor.RGB.SolveRGB(forergb, 'b'))); g.FillRectangle(b, new Rectangle(x, y, width, height));
                }
                else { SolidBrush b = new SolidBrush((color)); g.FillRectangle(b, new Rectangle(x,y, width, height)); }

            }
        }
    }
    public static class TerminalColor
    {
        public static class RGB
        {
            public static int SolveRGB(string rgb, char color)
            {
                string[] components = rgb.Split(',');
                if (components.Length != 3)
                {
                    throw new ArgumentException("Invalid RGB format");
                }
                if (color == 'r')
                {
                    return int.Parse(components[0]);
                }
                else if (color == 'g')
                {
                    return int.Parse(components[1]);
                }
                else if (color == 'b')
                {
                    return int.Parse(components[2]);
                }

                throw new ArgumentException("Invalid color");
            }
        }
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("gdi32.dll")]
        private static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);
        public static byte RedPixel(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint color = GetPixel(hdc,x, y);
            ReleaseDC(IntPtr.Zero, hdc);

            byte r = (byte)(color);
            return r;
        }
        public static byte GreenPixel(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint color = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);

            byte g = (byte)(color >> 8);
            return g;
        }
        public static byte BluePixel(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint color = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);

            byte b = (byte)(color >> 16);
            return b;
        }
        private static ConsoleColor GetForeColor()
        {
            return Console.ForegroundColor;
        }
        private static ConsoleColor GetBGColor()
        {
            return Console.BackgroundColor;
        }
        public static ConsoleColor BackColor(ConsoleColor color, bool setColor)
        {
            if (setColor)
            {
                Console.BackgroundColor = color;
                return color;
            }
            else { return GetBGColor(); }
        }
        public static ConsoleColor ForeColor(ConsoleColor color, bool setColor)
        {
            if(setColor)
            {
                Console.ForegroundColor = color;
                return color;
            }
            else { return GetForeColor(); }
        }
    }
    public static class Settings
    {
      

        [DllImport("user32.dll", SetLastError = true)]
        private  static extern bool SetWindowText(IntPtr hWnd, string lpString);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetClassLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        private const int MF_BYCOMMAND = 0x00000000;
        private const int SC_CLOSE = 0xF060;
        private const int SC_MINIMIZE = 0xF020;
        private const int SC_MAXIMIZE = 0xF030;
        private const int SC_SIZE = 0xF000;

        const int WM_SETICON = 0x0080;
        const int ICON_SMALL = 0;
        const int ICON_BIG = 1;

        [DllImport("user32.dll")]
        private static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        public static void SetIcon(bool taskbaricon, string path)
        {
            IntPtr consoleHandle = GetConsoleWindow();

            if (consoleHandle != IntPtr.Zero)
            {
                IntPtr hIcon = LoadIcon(path);
                if (hIcon != IntPtr.Zero && taskbaricon)
                {
                    SendMessage(consoleHandle, WM_SETICON, (IntPtr)ICON_SMALL, hIcon);
                    SendMessage(consoleHandle, WM_SETICON, (IntPtr)ICON_BIG, hIcon);
                }
                else if(hIcon != IntPtr.Zero && taskbaricon == false)
                {
                    SendMessage(consoleHandle, WM_SETICON, (IntPtr)ICON_SMALL, hIcon);
                }
            }
        }
        private static IntPtr LoadIcon(string iconPath)
        {
            try
            {
                return new System.Drawing.Icon(iconPath).Handle;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Simge yüklenirken bir hata oluştu: " + ex.Message);
                return IntPtr.Zero;
            }
        }
        public static void Title<t>(t title)
        {
            Console.Title = title.ToString();
        }
        public static void DisableButton(bool close,bool minimize,bool maximize, bool resize)
        {
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);
            if (handle != IntPtr.Zero)
            {
                if (close) DeleteMenu(sysMenu, SC_CLOSE, MF_BYCOMMAND);

                if (minimize) DeleteMenu(sysMenu, SC_MINIMIZE, MF_BYCOMMAND);

                if (maximize) DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);

                if (resize) DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
            }
        }

        public static void WindowWidth(int width)
        {
            Console.WindowWidth = width;
        }
        public static void WindowHeight(int height)
        {
            Console.WindowHeight = height;
        }
        public static int GetWindowWidth()
        {
            return Console.WindowWidth;
        }
        public static int GetWindowHeight()
        {
            return Console.WindowHeight;
        }
    }
    public static class App
    {
        public static void Start(string path)
        {
            Process.Start(path);
        }
    }
    public static class Recommend
    {
        private static int tutarimX;
        private static int tutarimY;
        public static string Box(int x, int y,  string word, string[] recom, string text, int lengt)
        {               
            Terminal.Cursor.SetCursorPositionXY(x, y);
            Terminal.Cursor.CursorVisibility(true);
            bool secim = false;
            bool main = true;
            bool yazim = true;
            string input = "";
            while(main)
            {
                int ileri = 0;
                while (yazim)
                {
                    string enUzun = recom.OrderByDescending(s => s.Length).FirstOrDefault();
                    ConsoleKeyInfo keys = Console.ReadKey(intercept: true);
                    char key = keys.KeyChar;

                    if (keys.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                        yazim = false;
                        main = false;
                    }
                    else if (key == '\b')
                    {
                        if (input.Length > 0)
                        {
                            Terminal.Cursor.CursorVisibility(false);
                            Terminal.Text.RemoveChar(Terminal.Cursor.GetCursorPositionX(), y - 1);
                            Terminal.Text.RemoveChar(Terminal.Cursor.GetCursorPositionX(), y + 1);
                            Terminal.Cursor.CursorVisibility(true);
                            Console.Write("\b \b");
                            input = input.Substring(0, input.Length - 1);

                        }
                        if (input.Contains(word))
                        {
                            int xx = Terminal.Cursor.GetCursorPositionX();
                            int yy = Terminal.Cursor.GetCursorPositionY();
                            for (int i = 0; i < recom.Length; i++)
                            {
                                Terminal.Cursor.SetCursorPositionXY(x, y + 2 + i);
                                Console.WriteLine(recom[i]);
                            }
                            Terminal.Cursor.SetCursorPositionXY(x, y - 2);
                            Console.Write(text);
                            if (enUzun.Length > input.Length)
                            {
                                for (int i = 0; i < enUzun.Length; i++)
                                {
                                    Terminal.Cursor.SetCursorPositionXY(x + i, y + 1);
                                    Console.Write("-");
                                }
                            }
                            else
                            {
                                for (int i = 0; i < input.Length; i++)
                                {
                                    Terminal.Cursor.SetCursorPositionXY(x + i, y + 1);
                                    Console.Write("-");
                                }
                            }
                            if (text.Length > input.Length)
                            {
                                for (int i = 0; i < text.Length; i++)
                                {
                                    Terminal.Cursor.SetCursorPositionXY(x + i, y - 1);
                                    Console.Write("-");
                                }
                            }
                            else
                            {
                                for (int i = 0; i < input.Length; i++)
                                {
                                    Terminal.Cursor.SetCursorPositionXY(x + i, y - 1);
                                    Console.Write("-");
                                }
                            }
                            Terminal.Cursor.SetCursorPositionXY(xx, yy);
                        }
                        else
                        {
                            Terminal.Text.RemoveText(x, x + enUzun.Length, y + 2, y + 2 + recom.Length);
                            Terminal.Text.RemoveText(x, x + text.Length, y - 2, y - 2);
                            Terminal.Text.RemoveText(x, input.Length > enUzun.Length ? input.Length : enUzun.Length, y + 1, y + 1);
                            Terminal.Text.RemoveText(x, x + text.Length, y - 1, y - 1);
                        }

                    }
                    else if (input.Length < lengt && (char.IsLetterOrDigit(key) || char.IsPunctuation(key) || char.IsSymbol(key) || char.IsWhiteSpace(key)))
                    {
                        input += key;
                        Console.Write(key);
                        if (input.Contains(word))
                        {
                            int xx = Terminal.Cursor.GetCursorPositionX();
                            int yy = Terminal.Cursor.GetCursorPositionY();
                            for (int i = 0; i < recom.Length; i++)
                            {
                                Terminal.Cursor.SetCursorPositionXY(x, y + 2 + i);
                                Console.WriteLine(recom[i]);
                            }
                            Terminal.Cursor.SetCursorPositionXY(x, y - 2);
                            Console.Write(text);
                            if (enUzun.Length > input.Length)
                            {
                                for (int i = 0; i < enUzun.Length; i++)
                                {
                                    Terminal.Cursor.SetCursorPositionXY(x + i, y + 1);
                                    Console.Write("-");
                                }
                            }
                            else
                            {
                                for (int i = 0; i < input.Length; i++)
                                {
                                    Terminal.Cursor.SetCursorPositionXY(x + i, y + 1);
                                    Console.Write("-");
                                }
                            }
                            if (text.Length > input.Length)
                            {
                                for (int i = 0; i < text.Length; i++)
                                {
                                    Terminal.Cursor.SetCursorPositionXY(x + i, y - 1);
                                    Console.Write("-");
                                }
                            }
                            else
                            {
                                for (int i = 0; i < input.Length; i++)
                                {
                                    Terminal.Cursor.SetCursorPositionXY(x + i, y - 1);
                                    Console.Write("-");
                                }
                            }
                            Terminal.Cursor.SetCursorPositionXY(xx, yy);
                        }
                        else
                        {
                            Terminal.Text.RemoveText(x, x + enUzun.Length, y + 2, y + 2 + recom.Length);
                            Terminal.Text.RemoveText(x, x + text.Length, y - 2, y - 2);
                            Terminal.Text.RemoveText(x, input.Length > enUzun.Length ? input.Length : enUzun.Length, y + 1, y + 1);
                            Terminal.Text.RemoveText(x, x + text.Length, y - 1, y - 1);
                        }
                    }
                    else if (input.Contains(word) && keys.Key == ConsoleKey.DownArrow)
                    {
                        tutarimX = Terminal.Cursor.GetCursorPositionX();
                        tutarimY = Terminal.Cursor.GetCursorPositionY();
                        if (Terminal.Cursor.GetCursorPositionY() == y)
                        {
                            Terminal.Text.RemoveText(x, recom[ileri].Length + x, y + 2, y + 2);
                            Terminal.Cursor.SetCursorPositionXY(x, y + 2);
                            Terminal.Text.WriteWithRGB("0,0,0,","255,255,255", recom[ileri]);
                        }
                        yazim = false;
                        secim = true;
                    }
                    Console.Title = ileri.ToString();
                }
                while (secim)
                {
                    ConsoleKeyInfo keys = Console.ReadKey(intercept: true);
                    char key = keys.KeyChar;
                    if (input.Contains(word) && keys.Key == ConsoleKey.DownArrow)
                    {
                        if (ileri + 1 != recom.Length)
                        {
                            ileri++;
                            Terminal.Text.RemoveText(x, recom[ileri].Length + x, Terminal.Cursor.GetCursorPositionY() + 1, Terminal.Cursor.GetCursorPositionY() + 1);                                                
                            Terminal.Cursor.SetCursorPositionXY(x, Terminal.Cursor.GetCursorPositionY() + 1);
                            Terminal.Text.WriteWithRGB("0,0,0,", "255,255,255", recom[ileri]);

                            if (ileri > 0)
                            {
                                Terminal.Text.RemoveText(x, recom[ileri - 1].Length + x, Terminal.Cursor.GetCursorPositionY() - 1, Terminal.Cursor.GetCursorPositionY() - 1);
                                Terminal.Cursor.SetCursorPositionXY(x, Terminal.Cursor.GetCursorPositionY() - 1);
                                Terminal.Text.WriteWithRGB("255, 255, 255", "0, 0, 0", recom[ileri - 1]);
                                Terminal.Cursor.SetCursorPositionXY(recom[ileri].Length + x, Terminal.Cursor.GetCursorPositionY() + 1);
                            }

                        }

                    }
                    else if (input.Contains(word) && keys.Key == ConsoleKey.UpArrow)
                    {
                        if (Terminal.Cursor.GetCursorPositionY() - 1 != y && ileri - 1 >= 0)
                        {
                            ileri--;
                            Terminal.Cursor.SetCursorPositionXY(recom[ileri].Length + x, Terminal.Cursor.GetCursorPositionY() - 1);
                            Terminal.Text.RemoveText(x, recom[ileri + 1].Length + x, Terminal.Cursor.GetCursorPositionY() + 1, Terminal.Cursor.GetCursorPositionY() + 1);
                            Terminal.Cursor.SetCursorPositionXY(x, Terminal.Cursor.GetCursorPositionY() + 1);
                            Terminal.Text.WriteWithRGB("255, 255, 255", "0, 0, 0", recom[ileri + 1]);
                            Terminal.Cursor.SetCursorPositionXY(recom[ileri].Length + x, Terminal.Cursor.GetCursorPositionY() - 1);
                            Terminal.Text.RemoveText(x, recom[ileri].Length + x, Terminal.Cursor.GetCursorPositionY(), Terminal.Cursor.GetCursorPositionY());
                            Terminal.Cursor.SetCursorPositionXY(x, Terminal.Cursor.GetCursorPositionY());
                            Terminal.Text.WriteWithRGB("0,0,0,", "255,255,255", recom[ileri]);
                        }

                    }
                    else if (keys.Key == ConsoleKey.Enter)
                    {
                        for (int i = 0; i < recom.Length; i++)
                        {
                            Terminal.Cursor.SetCursorPositionXY(x, y + 2 + i);
                            Console.WriteLine(recom[i]);
                        }
                        if(input.Length < lengt)
                        {
                            if (!(recom[ileri].Length + input.Length > lengt))
                            {
                                Terminal.Cursor.SetCursorPositionXY(tutarimX, tutarimY);
                                input += " " + recom[ileri];
                                Console.Write(" " + recom[ileri]);
                            }
                        }
                        Terminal.Cursor.SetCursorPositionXY(x + input.Length, y);

                        yazim = true;
                        secim = false;
                    }
                }
            }
            return input;
        }
    }
    public static class Table
    {
        public static void CreateTable(string title, string[] items, int x, int y)
        {
            string[] tutabisi = items;
            Terminal.Cursor.SetCursorPositionXY(x, y);
            Console.WriteLine(title);
            string tut = "";
            for(int i = 0; i < items.Length;i++)
            {
                string a = i + ". ";

                items[i] = a + items[i].ToString();
            }
            string enUzun = items.OrderByDescending(s => s.Length).FirstOrDefault();
            if (title.Length > enUzun.Length)
            {
                for (int i = 0; i < title.Length; i++)
                {
                    tut += "-";
                }
            }
            else
            {
                for (int i = 0; i < enUzun.Length; i++)
                {
                    tut += "-";
                }
            }
            Terminal.Cursor.SetCursorPositionXY(x, y + 1);
            Console.Write(tut);
            Console.WriteLine();
            for (int i = 0; i < items.Length;i++)
            {
                Terminal.Cursor.SetCursorPositionXY(x, y + i + 2);
                Console.WriteLine(items[i]);
            }
            items = tutabisi;
        }
    }
    public static class Time
    {
        public static int GetHour()
        {
            return DateTime.Now.Hour;
        }
        public static int GetMinute()
        {
            return DateTime.Now.Minute;
        }
        public static int GetMillisecond()
        {
            return DateTime.Now.Millisecond;
        }
        public static int GetSecond()
        {
            return DateTime.Now.Second;
        }
        public static int GetMonth()
        {
            return DateTime.Now.Month;
        }
        public static int GetYear()
        {
            return DateTime.Now.Year;
        }
        public static DayOfWeek GetDayOfWeek()
        {
            return DateTime.Now.DayOfWeek;
        }
        public static int GetDayOfYear()
        {
            return DateTime.Now.DayOfYear;
        }
        public static int GetDay()
        {
            return DateTime.Now.Day;
        } 
        public static void Wait(int delay)
        {
            Thread.Sleep(delay);
        }
    }
    public static class Text
    {
        public static int IndexOf(string text,string val,int index){return text.IndexOf(val,index);}
        public static bool Contains<t>(string text, t val){if(text.Contains(val.ToString())) return true;else return false;}
        public static string MakeString<t>(t obj){return obj.ToString();}
        public static string LowerCase(string text){return text.ToLower();}
        public static string UpperCase(string text){return text.ToUpper();}
        public static string Replace(string Text,string oldVal,string newVal){return Text.Replace(oldVal,newVal);}
        public static void OutputEncoding(Encoding encoder){Console.OutputEncoding = encoder;}
        public static void InputEncoding(Encoding encoder){Console.InputEncoding = encoder;}
        public static int LengthOf(string text){ return text.Length; }
        [StructLayout(LayoutKind.Sequential)]
        private struct COORD
        {
            public short X;
            public short Y;
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadConsoleOutputCharacter(IntPtr hConsoleOutput, [Out] char[] lpCharacter, uint nLength, COORD dwReadCoord, out uint lpNumberOfCharsRead);

        const int STD_OUTPUT_HANDLE = -11;
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleMode(IntPtr handle, out int mode);
        const int STD_INPUT_HANDLE = -10;
        const uint ENABLE_MOUSE_INPUT = 0x0010;
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
        public static void Write<t>(t text)
        {
            Console.Write(text);
        }
        public static void WriteLine<t>(t text)
        {
            Console.WriteLine(text);
        }
        public static void SetWrite<t>(t text,int x, int y)
        {
            Cursor.SetCursorPositionXY(x, y);
            Console.Write(text);
        }
        public static void SetWriteLine<t>(t text, int x, int y)
        {
            Cursor.SetCursorPositionXY(x, y);
            Console.WriteLine(text);
        }
        public static void NewLine()
        {
            Console.WriteLine();
        }
        public static char GetChar(int x, int y)
        {
            IntPtr hConsoleOutput = GetStdHandle(STD_OUTPUT_HANDLE);
            COORD readCoord = new COORD { X = (short)x, Y = (short)y };
            char[] lpCharacter = new char[1];
            uint lpNumberOfCharsRead;
            ReadConsoleOutputCharacter(hConsoleOutput, lpCharacter, 1, readCoord, out lpNumberOfCharsRead);
            return lpCharacter[0];
        }
        public static string GetText(int start_x,int end_x,int start_y, int end_y)
        {
            string text = "";
            for(int i = start_x; i < end_x + 1;i++)
            {
                text += GetChar(i, start_y).ToString();
                if(i == end_x)
                {
                    if(start_y + 1 == end_y)
                    {
                        text += "\n";
                        start_y += 1;
                        i = start_x - 1;
                    }
                }              
            }
            return text;

        }
        public static void WriteArea<t>(t chr, int start_x, int end_x, int start_y, int end_y, bool color = false, bool fillinside = false,string forergb = "0,0,0", string bgrgb = "255,255,255")
        {
            int xx = Terminal.Cursor.GetCursorPositionX();
            int yy = Terminal.Cursor.GetCursorPositionY();
            if(color == false && fillinside == true)
            {
                for(int i = start_x; i < end_x + 1; i++)
                {
                    Terminal.Cursor.SetCursorPositionXY(i,start_y);
                    Terminal.Text.Write(chr);
                }
                for (int i = start_y; i < end_y + 1; i++)
                {
                    Terminal.Cursor.SetCursorPositionXY(start_x, i);
                    Terminal.Text.Write(chr);
                    Terminal.Cursor.SetCursorPositionXY(start_x + end_x, i);
                    Terminal.Text.Write(chr);
                }
                for (int i = start_x; i < end_x + 1; i++)
                {
                    Terminal.Cursor.SetCursorPositionXY(i, end_y);
                    Terminal.Text.Write(chr);
                }
            }
            else if (color == true && fillinside == true)
            {
                for (int i = start_x; i < end_x + 1; i++)
                {
                    Terminal.Text.SetWriteWithRGB(i,start_y,forergb,bgrgb, chr.ToString());
                }
                for (int i = start_y; i < end_y + 1; i++)
                {
                    Terminal.Text.SetWriteWithRGB(start_x, i, forergb, bgrgb, chr.ToString());
                    Terminal.Text.SetWriteWithRGB(start_x + end_x, i, forergb, bgrgb, chr.ToString());
                }
                for (int i = start_x; i < end_x + 1; i++)
                {
                    Terminal.Text.SetWriteWithRGB(i, end_y, forergb, bgrgb, chr.ToString());
                }
            }
            else
            {
                for (int i = start_y; i < end_y + 1; i++)
                {
                    for (int j = start_x; j < end_x + 1; j++)
                    {
                        SetWriteWithRGB(j, i, forergb, bgrgb, chr.ToString());
                    }
                }
            }   
            Terminal.Cursor.SetCursorPositionXY(xx, yy);
        }

        public static void WriteWithRGB(string forergb = "0,0,0", string bgrgb = "255,255,255", string text = "")
        {
            var handle = GetStdHandle(-11);
            int mode;
            GetConsoleMode(handle, out mode);
            SetConsoleMode(handle, mode | 0x4);
            Console.Write($"\x1b[48;2;{TerminalColor.RGB.SolveRGB(bgrgb, 'r')};{TerminalColor.RGB.SolveRGB(bgrgb, 'g')};{TerminalColor.RGB.SolveRGB(bgrgb, 'b')};38;2;{TerminalColor.RGB.SolveRGB(forergb, 'r')};{TerminalColor.RGB.SolveRGB(forergb, 'g')};{TerminalColor.RGB.SolveRGB(forergb, 'b')}m{text}\x1b[0m");
        }
        public static void WriteLineWithRGB(string forergb = "0,0,0", string bgrgb = "255,255,255", string text = "")
        {
            var handle = GetStdHandle(-11);
            int mode;
            GetConsoleMode(handle, out mode);
            SetConsoleMode(handle, mode | 0x4);
            Console.WriteLine($"\x1b[48;2;{TerminalColor.RGB.SolveRGB(bgrgb, 'r')};{TerminalColor.RGB.SolveRGB(bgrgb, 'g')};{TerminalColor.RGB.SolveRGB(bgrgb, 'b')};38;2;{TerminalColor.RGB.SolveRGB(forergb, 'r')};{TerminalColor.RGB.SolveRGB(forergb, 'g')};{TerminalColor.RGB.SolveRGB(forergb, 'b')}m{text}\x1b[0m");
        }
        public static void SetWriteWithRGB(int x, int y, string forergb = "0,0,0", string bgrgb = "255,255,255", string text = "")
        {
            var handle = GetStdHandle(-11);
            int mode;
            GetConsoleMode(handle, out mode);
            SetConsoleMode(handle, mode | 0x4);
            Terminal.Cursor.SetCursorPositionXY(x, y);
            Console.Write($"\x1b[48;2;{TerminalColor.RGB.SolveRGB(bgrgb, 'r')};{TerminalColor.RGB.SolveRGB(bgrgb, 'g')};{TerminalColor.RGB.SolveRGB(bgrgb, 'b')};38;2;{TerminalColor.RGB.SolveRGB(forergb, 'r')};{TerminalColor.RGB.SolveRGB(forergb, 'g')};{TerminalColor.RGB.SolveRGB(forergb, 'b')}m{text}\x1b[0m");
        }
        public static void SetWriteLineWithRGB(int x, int y, string forergb = "0,0,0", string bgrgb = "255,255,255", string text = "")
        {
            var handle = GetStdHandle(-11);
            int mode;
            GetConsoleMode(handle, out mode);
            SetConsoleMode(handle, mode | 0x4); Terminal.Cursor.SetCursorPositionXY(x, y);
            Console.WriteLine($"\x1b[48;2;{TerminalColor.RGB.SolveRGB(bgrgb, 'r')};{TerminalColor.RGB.SolveRGB(bgrgb, 'g')};{TerminalColor.RGB.SolveRGB(bgrgb, 'b')};38;2;{TerminalColor.RGB.SolveRGB(forergb, 'r')};{TerminalColor.RGB.SolveRGB(forergb, 'g')};{TerminalColor.RGB.SolveRGB(forergb, 'b')}m{text}\x1b[0m");
        }

        public static void Capsule(int x, int y,string text, string forergb = "0,0,0", string bgrgb = "255,255,255")
        {
            for(int i = 0; i < text.Length;i++)
            { 
                if(i == 0)
                {
                    Terminal.Cursor.SetCursorPositionXY(x - 1, y);
                    Terminal.Text.WriteWithRGB(forergb, bgrgb, "╭");
                    Terminal.Cursor.SetCursorPositionXY(x, y);
                    Terminal.Text.WriteWithRGB(forergb, bgrgb, "─");
                    Terminal.Cursor.SetCursorPositionXY(x + 1, y);
                    Terminal.Text.WriteWithRGB(forergb, bgrgb, "╮");
                }
                Terminal.Cursor.SetCursorPositionXY(x, y + i + 1);
                if(i < text.Length)
                {
                    Terminal.Text.Write(text[i]);
                } 
                Terminal.Cursor.SetCursorPositionXY(x - 1, y + i + 1);
                Terminal.Text.WriteWithRGB(forergb, bgrgb, "│");
                Terminal.Cursor.SetCursorPositionXY(x + 1, y + i + 1);
                Terminal.Text.WriteWithRGB(forergb, bgrgb, "│");
                if(i  + 1 == text.Length)
                {
                    Terminal.Cursor.SetCursorPositionXY(x - 1, y + i + 2);
                    Terminal.Text.WriteWithRGB(forergb, bgrgb, "╰");
                    Terminal.Cursor.SetCursorPositionXY(x, y + i + 2);
                    Terminal.Text.WriteWithRGB(forergb, bgrgb, "─");
                    Terminal.Cursor.SetCursorPositionXY(x + 1, y + 2 +i);
                    Terminal.Text.WriteWithRGB(forergb, bgrgb, "╯");
                }
            }
        }
        public static void RemoveChar(int x, int y)
        {
            int xx = Terminal.Cursor.GetCursorPositionX();
            int yy = Terminal.Cursor.GetCursorPositionY();
            Terminal.Cursor.SetCursorPositionXY(x, y);
            Console.Write("\b \b");
            Terminal.Cursor.SetCursorPositionXY(xx, yy);
        }
        public static void RemoveText(int start_x, int end_x, int start_y, int end_y)
        {
            int xx = Terminal.Cursor.GetCursorPositionX();
            int yy = Terminal.Cursor.GetCursorPositionY();
            for (int i = start_y; i < end_y + 1; i++)
            {
                for (int j = start_x; j < end_x + 1; j++)
                {
                    RemoveChar(j, i);
                }
            } 
            Terminal.Cursor.SetCursorPositionXY(xx, yy);
        }
    }
    public static class FileIO
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr CreateFile(string lpFileName, FileAccess dwDesiredAccess, FileShare dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition, FileAttributes dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetFileSizeEx(IntPtr hFile, out long lpFileSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hObject);

        enum FileAccess : uint
        {
            GENERIC_READ = 0x80000000,
            GENERIC_WRITE = 0x40000000
        }

        enum FileShare : uint
        {
            FILE_SHARE_READ = 0x00000001,
            FILE_SHARE_WRITE = 0x00000002,
            FILE_SHARE_DELETE = 0x00000004
        }

        enum FileMode : uint
        {
            OPEN_EXISTING = 3
        }
        public static void SetRowFromFile(string filepath, int row, string newtext)
        {
            if (!File.Exists(filepath) || row <= 0)
            {
                return;
            }
            string[] satirlar = File.ReadAllLines(filepath);

            if (row > satirlar.Length)
            {
                return;
            }
            satirlar[row - 1] = newtext;
            File.WriteAllLines(filepath, satirlar);
        }
        public static void AddTextoToRowFromFile(string filepath, int row, int index, string text)
        {

            if (!File.Exists(filepath) || row<= 0)
            {
                return;
            }
            string[] satirlar = File.ReadAllLines(filepath);
            if (row > satirlar.Length)
            {

                return;
            }
            string satirMetni = satirlar[row - 1];
            if (index < 0 || index > satirMetni.Length)
            {

                satirMetni = satirMetni.PadRight(index + text.Length);
            }
            satirMetni = satirMetni.Insert(index, text);
            satirlar[row - 1] = satirMetni;
            File.WriteAllLines(filepath, satirlar);
        }

        public static string GetRowFromFile(string filepath, int row)
        {
            if (!File.Exists(filepath) || row <= 0)
            {
                return "b";
            }
            string[] satirlar = File.ReadAllLines(filepath);

            if (row > satirlar.Length)
            {
                return "ds0";
            }
            return satirlar[row - 1];
        }
    }
    public static class Cursor
    {
        public static void CursorVisibility(bool visibility) { Console.CursorVisible = visibility; }
        public static void SetCursorPositionXY(int x, int y)
        {
            Console.SetCursorPosition(x, y);
        }
        public static void SetCursorPositionY(int y) { Console.CursorTop = y; }
        public static void SetCursorPositionX(int x) { Console.CursorLeft = x; }

        public static string GetCursorPositionXY()
        {
            int x = GetCursorPositionX();
            int y = GetCursorPositionY();
            return "X {" + x + "} Y {" + y + "}";
        }
        public static int GetCursorPositionY() { return Console.CursorTop; }
        public static int GetCursorPositionX() { return Console.CursorLeft; }
    }
    public static class Mouse
    {
        public static class GlobalMouse
        {     
            [DllImport("user32.dll")]
            public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);
            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern void mouse_event(uint dwFlags, int dx, int dy, uint cButtons, uint dwExtraInfo);
            private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
            private const int MOUSEEVENTF_RIGHTUP = 0x10;
            private const int MOUSEEVENTF_LEFTDOWN = 0x02;
            private const int MOUSEEVENTF_LEFTUP = 0x04;

            [DllImport("user32.dll")]
            private static extern short GetAsyncKeyState(int vKey);

            [DllImport("user32.dll")]
            private static extern bool GetCursorPos(out POINT lpPoint);

            const int VK_LBUTTON = 0x01; 
            const int VK_RBUTTON = 0x02; 

            static bool previousLeftState = false;
            static bool previousRightState = false;

            private struct POINT
            {
                public int X;
                public int Y;
            }
            [DllImport("user32.dll")]
            private static extern bool SetCursorPos(int X, int Y);
            public static int GetMouseX() {POINT point; GetCursorPos(out point); return point.X; }
            public static int GetMouseY() { POINT point; GetCursorPos(out point); return point.Y; }
            public static string GetMouseXY() { POINT point; GetCursorPos(out point); int x = Terminal.Mouse.GlobalMouse.GetMouseX();int y = Terminal.Mouse.GlobalMouse.GetMouseY();return "X {" + x + "} Y {" + y + "}";}
            public static void SetMouseXY(int x, int y)
            {
                SetCursorPos(x, y);
            }
            public static void SendRightClick(int x, int y, int delayToMouseUp = 0)
            {
                mouse_event(MOUSEEVENTF_RIGHTDOWN, x, y, 0, 0);
                Terminal.Time.Wait(delayToMouseUp);
                mouse_event(MOUSEEVENTF_RIGHTUP, x, y, 0, 0);
            }
            public static void SendLeftClick(int x, int y, int delayToMouseUp = 0)
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0);
                Thread.Sleep(delayToMouseUp);
                mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0);
            }
            public static bool LeftClick(bool MouseUp = false)
            {
                bool currentLeftState = (GetAsyncKeyState(VK_LBUTTON) & 0x8000) != 0;
                if (!previousLeftState && currentLeftState && MouseUp == false)
                {
                    return true;
                }
                else if (previousLeftState && !currentLeftState && MouseUp == true)
                {
                    return true;
                }
                else { return false; }
            }
            public static bool RigthClick(bool MouseUp = false)
            {
                bool currentRightState = (GetAsyncKeyState(VK_RBUTTON) & 0x8000) != 0;
                if (!previousRightState && currentRightState && MouseUp == false)
                {
                    return true;
                }
                else if (previousRightState && !currentRightState && MouseUp == true)
                {
                    return true;
                }
                else { return false; }
            }
            public static bool MouseHover(int start_x, int end_x,int start_y, int end_y)
            {
                int xx = Mouse.GlobalMouse.GetMouseX();
                int yy = Mouse.GlobalMouse.GetMouseY();
                if(xx >= end_x && xx <= end_x && yy >= end_y && yy <= end_y)
                {
                    return true;
                }
                else return false;
            }
        }
       public static class ConsoleMouse
        {
            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern IntPtr GetStdHandle(int handle);

            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern bool ReadConsoleInput(IntPtr hConsoleInput, [Out] INPUT_RECORD[] lpBuffer, uint nLength, out uint lpNumberOfEvents);

            const int STD_INPUT_HANDLE = -10;
            const uint ENABLE_EXTENDED_FLAGS = 0x0080;
            const uint ENABLE_MOUSE_INPUT = 0x0010;

            [StructLayout(LayoutKind.Explicit)]
            private struct INPUT_RECORD
            {
                [FieldOffset(0)]
                public ushort EventType;
                [FieldOffset(4)]
                public MOUSE_EVENT_RECORD MouseEvent;
            }
            private struct MOUSE_EVENT_RECORD
            {
                public COORD dwMousePosition;
                public uint dwButtonState;
                public uint dwControlKeyState;
                public uint dwEventFlags;
            }

            private struct COORD
            {
                public short X;
                public short Y;
            }
            public static string MouseXY()
            {
                int x = GetMouseX();
                int y = GetMouseY();
                return "X {" + x + "} Y {" + y + "}";
            }
            public static int GetMouseX()
            {
                IntPtr hConsoleHandle = GetStdHandle(STD_INPUT_HANDLE);
                SetConsoleMode(hConsoleHandle, ENABLE_MOUSE_INPUT | ENABLE_EXTENDED_FLAGS);
                INPUT_RECORD[] records = new INPUT_RECORD[1];
                uint numberOfEvents;
                ReadConsoleInput(hConsoleHandle, records, 1, out numberOfEvents);
                MOUSE_EVENT_RECORD mouseEvent = records[0].MouseEvent;
                COORD mousePosition = mouseEvent.dwMousePosition;
                return mousePosition.X;
            }

            public static int GetMouseY()
            {
                IntPtr hConsoleHandle = GetStdHandle(STD_INPUT_HANDLE);
                SetConsoleMode(hConsoleHandle, ENABLE_MOUSE_INPUT | ENABLE_EXTENDED_FLAGS);
                INPUT_RECORD[] records = new INPUT_RECORD[1];
                uint numberOfEvents;
                ReadConsoleInput(hConsoleHandle, records, 1, out numberOfEvents);
                MOUSE_EVENT_RECORD mouseEvent = records[0].MouseEvent;
                COORD mousePosition = mouseEvent.dwMousePosition;
                return mousePosition.Y;
            }
            public static bool LeftClickPosition(int x, int y)
            {
                IntPtr hConsoleHandle = GetStdHandle(STD_INPUT_HANDLE);
                SetConsoleMode(hConsoleHandle, ENABLE_MOUSE_INPUT | ENABLE_EXTENDED_FLAGS);
                INPUT_RECORD[] records = new INPUT_RECORD[1];
                uint numberOfEvents;
                ReadConsoleInput(hConsoleHandle, records, 1, out numberOfEvents);
                if (numberOfEvents > 0 && records[0].EventType == 0x0002)
                {
                    MOUSE_EVENT_RECORD mouseEvent = records[0].MouseEvent;
                    COORD mousePosition = mouseEvent.dwMousePosition;
                    if ((mouseEvent.dwButtonState & 0x01) != 0 && mousePosition.X == x && mousePosition.Y == y)
                    {
                        return true;
                    }
                }
                return false;
            }
            public static bool RigthClickPosition(int x, int y)
            {
                IntPtr hConsoleHandle = GetStdHandle(STD_INPUT_HANDLE);
                SetConsoleMode(hConsoleHandle, ENABLE_MOUSE_INPUT | ENABLE_EXTENDED_FLAGS);
                INPUT_RECORD[] records = new INPUT_RECORD[1];
                uint numberOfEvents;
                ReadConsoleInput(hConsoleHandle, records, 1, out numberOfEvents);
                if (numberOfEvents > 0 && records[0].EventType == 0x0002)
                {
                    MOUSE_EVENT_RECORD mouseEvent = records[0].MouseEvent;
                    COORD mousePosition = mouseEvent.dwMousePosition;
                    if ((mouseEvent.dwButtonState & 0x02) != 0 && mousePosition.X == x && mousePosition.Y == y)
                    {
                        return true;
                    }
                }
                return false;
            }
            public static bool LeftClick()
            {
                IntPtr hConsoleHandle = GetStdHandle(STD_INPUT_HANDLE);
                SetConsoleMode(hConsoleHandle, ENABLE_MOUSE_INPUT | ENABLE_EXTENDED_FLAGS);
                INPUT_RECORD[] records = new INPUT_RECORD[1];
                uint numberOfEvents;
                ReadConsoleInput(hConsoleHandle, records, 1, out numberOfEvents);
                if (numberOfEvents > 0 && records[0].EventType == 0x0002)
                {
                    MOUSE_EVENT_RECORD mouseEvent = records[0].MouseEvent;
                    COORD mousePosition = mouseEvent.dwMousePosition;
                    if ((mouseEvent.dwButtonState & 0x01) != 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            public static bool RigthClick()
            {
                IntPtr hConsoleHandle = GetStdHandle(STD_INPUT_HANDLE);
                SetConsoleMode(hConsoleHandle, ENABLE_MOUSE_INPUT | ENABLE_EXTENDED_FLAGS);
                INPUT_RECORD[] records = new INPUT_RECORD[1];
                uint numberOfEvents;
                ReadConsoleInput(hConsoleHandle, records, 1, out numberOfEvents);
                if (numberOfEvents > 0 && records[0].EventType == 0x0002)
                {
                    MOUSE_EVENT_RECORD mouseEvent = records[0].MouseEvent;
                    COORD mousePosition = mouseEvent.dwMousePosition;
                    if ((mouseEvent.dwButtonState & 0x02) != 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            public static bool MouseHover(int x_start, int x_end, int y_start, int y_end)
            {
                IntPtr hConsoleHandle = GetStdHandle(STD_INPUT_HANDLE);
                SetConsoleMode(hConsoleHandle, ENABLE_MOUSE_INPUT | ENABLE_EXTENDED_FLAGS);
                INPUT_RECORD[] records = new INPUT_RECORD[1];
                uint numberOfEvents;
                ReadConsoleInput(hConsoleHandle, records, 1, out numberOfEvents);
                MOUSE_EVENT_RECORD mouseEvent = records[0].MouseEvent;
                COORD mousePosition = mouseEvent.dwMousePosition;
                int x = GetMouseX();
                int y = GetMouseY();
                if (x >= x_start && x <= x_end && y >= y_start && y <= y_end)
                {
                    return true;
                }
                else return false;
            }
        }
    }
}