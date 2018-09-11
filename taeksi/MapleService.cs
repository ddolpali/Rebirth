﻿using System;
using Common.Server;

namespace taeksi
{
    /// <summary>
    /// Tiny wrapper for easy insertion into a windows service planned for the future.
    /// </summary>
    public sealed class MapleService : IDisposable
    {
        public WvsCenter WvsCenter { get; }

        public MapleService()
        {
            WvsCenter = new WvsCenter(1);
        }

        public void Start() => WvsCenter.Start();
        public void Stop() => WvsCenter.Stop();

        public void Dispose()
        {
            WvsCenter?.Dispose();
        }
    }
}
