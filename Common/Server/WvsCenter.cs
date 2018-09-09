using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Log;
using Microsoft.Extensions.Configuration;
using PKG1;
using WvsRebirth;

namespace Common.Server
{
    public class WvsCenter
    {
        private readonly WvsLogin m_login;
        private readonly WvsGame[] m_games;

        public readonly PackageCollection WzProvider;

        public static IConfiguration config;

        public WvsCenter(int channels)
        {
            config = new ConfigurationBuilder()
                .AddJsonFile("Global.json", optional: true, reloadOnChange: true)
                .Build();

            WZReader.InitializeKeys();

            WzProvider = new PackageCollection(Constants.WzLocation);

            m_login = new WvsLogin(this);
            m_games = new WvsGame[channels];

            for (int i = 0; i < m_games.Length; i++)
            {
                var channel = (byte) i;
                m_games[i] = new WvsGame(this,channel);
            }
        }

        public void Start()
        {
            m_login.Start();
            m_games.ToList().ForEach(x => x.Start());
        }
        public void Stop()
        {
            m_login.Stop();
            m_games.ToList().ForEach(x => x.Stop());
        }
    }
}
