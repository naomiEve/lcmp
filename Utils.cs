using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace lcmp
{
    class Utils
    {
        /*Utilities to make life easier, i guess?
                        lcmp 1.0               */
        public static void writeColoredLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void writeColored(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void writeCenteredColLine(string text, ConsoleColor color)
        {
            try { text = Path.GetFileName(text); }
            catch { /*it isn't a file name then. :p*/ }
            Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, Console.CursorTop);
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
