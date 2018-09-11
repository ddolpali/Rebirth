using System;
using Common.Log;

namespace taeksi
{
    class taeksi
    {
        static void Main(string[] args)
        {
            var mapleSvc = new MapleService();

            if (Environment.UserInteractive)
            {
                ConsoleLog.InitConsole("taeksi v95");
                Logger.Add(new ConsoleLog());

                mapleSvc.Start();
                Console.ReadLine();
                mapleSvc.Stop();
            }
            else
            {
                //TODO: Window Service Code
            }
        }
    }
}
