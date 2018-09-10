using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace Common.Log
{
    public enum LogLevel
    {
        Error = 0,
        Warning,
        Info,
        Debug,
        Trace
    }
    public interface ILogger : IDisposable
    {
        void Write(string format, params object[] objects);
        void Write(LogLevel logLevel, string format, params object[] objects);
    }
    public static class Logger
    {
        private static readonly object Locker = new object();
        private static readonly List<ILogger> m_loggers;

        public static int Count => m_loggers.Count;

        static Logger()
        {
            m_loggers = new List<ILogger>();
        }

        public static void Add(ILogger log)
        {
            m_loggers.Add(log);
        }
        public static void Remove(ILogger log)
        {
            m_loggers.Remove(log);
        }
        public static void Clear()
        {
            m_loggers.Clear();
        }

        public static void Write(string format, params object[] objects)
        {
            m_loggers.ForEach(x => x.Write(format, objects));
        }
        public static void Write(LogLevel logLevel, string format, params object[] objects)
        {
            m_loggers.ForEach(x => x.Write(logLevel, format, objects));
        }
        public static void Exception(Exception ex)
        {
            string txt = string.Format("[{0}]-----------------{1}{2}{1}", DateTime.Now, Environment.NewLine, ex);

            try
            {
                lock (Locker)
                {
                    File.AppendAllText("EXCEPTIONS.txt", txt);
                }
            }
            catch (Exception nex)
            {
                Debug.WriteLine("Exception: {0}", nex);
            }
        }
        public static void Exception(Exception ex, string fmt, params object[] args)
        {
            Exception(ex);
            Write(LogLevel.Error, fmt, args);
        }
        public static void Trace(string message, [CallerMemberName] string name = "", [CallerLineNumber] int line = 0)
        {
            string txt = $"{name}\t{line} : {message}";

            try
            {
                lock (Locker)
                {
                    File.AppendAllText("TRACE.txt", txt);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Trace: {0}", ex);
            }
        }
        public static void Dispose()
        {
            m_loggers.ForEach(x => x.Dispose());
            m_loggers.Clear();
        }
    }
}