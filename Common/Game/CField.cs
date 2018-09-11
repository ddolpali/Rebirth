﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Client;
using Common.Entities;
using Common.Log;
using Common.Network;
using Common.Packets;
using Common.Server;
using PKG1;

namespace Common.Game
{
    public sealed class CField
    {

        private readonly bool m_sentController = false; //TODO: rEMOVE THIS ONE DAY LOL


        public int MapId { get; }

        public List<CharacterData> Characters { get; }
        public Dictionary<CharacterData, WvsGameClient> Sockets { get; }

        public CPortalMan Portals { get; }
        public CFootholdMan Footholds { get; }
        public CMobPool Mobs { get; }
        public CNpcPool Npcs { get; set; }

        private CField(int mapId)
        {
            MapId = mapId;

            Characters = new List<CharacterData>();
            Sockets = new Dictionary<CharacterData, WvsGameClient>();

            Portals = new CPortalMan();
            Footholds = new CFootholdMan();
            Mobs = new CMobPool();
            Npcs = new CNpcPool();
        }

        public void Add(WvsGameClient c)
        {
            var character = c.Character;

            if (c.SentCharData)
            {
                c.SendPacket(CPacket.SetField(character, false, c.ChannelId));
            }
            else
            {
                c.SentCharData = true;
#if DEBUG
                //So me myself and i all spawn close to eachother <3
                character.Stats.nPortal = 0;
#else
                character.Stats.nPortal = Portals.GetRandomSpawn();
#endif
                c.SendPacket(CPacket.SetField(character, true, c.ChannelId));
            }

            //Send client being added all the existing characters in the map
            Characters.ForEach(x => c.SendPacket(CPacket.UserEnterField(x)));

            SendSpawnMobs(c);
            SendSpawnNpcs(c);

            Characters.Add(c.Character);
            Sockets.Add(c.Character, c);

            //Broadcast everyone already in the map that you have arrived
            Broadcast(CPacket.UserEnterField(character), c);
        }
        public void Remove(WvsGameClient c)
        {
            var character = c.Character;
            Broadcast(CPacket.UserLeaveField(character), c);

            Characters.Remove(c.Character);
            Sockets.Remove(c.Character);
        }

        public void Broadcast(COutPacket packet)
        {
            foreach (var kvp in Sockets)
            {
                kvp.Value.SendPacket(packet);
            }

            packet.Dispose();
        }
        public void Broadcast(COutPacket packet, params WvsGameClient[] excludes)
        {
            foreach (var kvp in Sockets)
            {
                if (!excludes.Contains(kvp.Value))
                    kvp.Value.SendPacket(packet);
            }

            packet.Dispose();
        }

        public void SendSpawnMobs(WvsGameClient c)
        {
            foreach (var mob in Mobs)
            {
                if (mob.Controller == 0 || mob.Controller == c.Character.CharId)
                {
                    mob.Controller = c.Character.CharId;
                    c.SendPacket(CPacket.MobChangeController(mob, 1));
                }
                
                c.SendPacket(CPacket.MobEnterField(mob));
            }
        }
        public void SendSpawnNpcs(WvsGameClient c)
        {
            foreach (var npc in Npcs)
            {
                c.SendPacket(CPacket.NpcEnterField(npc));
            }
        }
        
        public static CField Load(int mapId, WvsCenter parentServer)
        {
            var WzProvider = parentServer.WzProvider;
            var wz = "Map";
            var path = $"Map/Map{mapId / 100000000}/{mapId}.img";

            try
            {
                var mapNode = WzProvider.Resolve($"{wz}/{path}");

                var cf = new CField(mapId);
                cf.Load(mapNode);
                return cf;
            }
            catch (KeyNotFoundException knfe)
            {
                return null;
            }
        }
        
        private void Load(WZProperty mapNode)
        {
            Portals.Load(mapNode);
            Footholds.Load(mapNode);
            Npcs.Load(mapNode);

            Mobs.Load(mapNode);
            Mobs.DoMobLogic();
        }
    }
}
