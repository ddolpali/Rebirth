using System;

namespace Common.Log
{
    public class ConsoleLog : ILogger
    {
        public static bool LogDateTime { get; set; } = false;

        private static readonly object Locker = new object();

        private static readonly ConsoleColor[] LogColors =
        {
            ConsoleColor.Red        ,
            ConsoleColor.Yellow     ,
            ConsoleColor.Green      ,
            ConsoleColor.Cyan       ,
            ConsoleColor.Magenta
        };


        public static void InitConsole(string title)
        {
            Console.Title = title;
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Write(LogLevel logLevel, string message)
        {
            lock (Locker)
            {
                if (LogDateTime)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("[{0}] ", DateTime.Now);
                }

                Console.ForegroundColor = LogColors[(int)logLevel];
                Console.Write("[{0}] ", logLevel);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(message);
            }
        }

        public void Write(string format, params object[] objects)
        {
            Write(LogLevel.Info, format, objects);
        }
        public void Write(LogLevel logLevel, string format, params object[] objects)
        {
            var msg = objects.Length > 0 ? string.Format(format, objects) : format;
            Write(logLevel, msg);
        }

        public void Dispose()
        {
        }
    }
}
