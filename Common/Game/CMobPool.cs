using System;
using System.Collections.Generic;
using PKG1;

namespace Common.Game
{
    public class CMobPool : CObjectPool<int, CMob>
    {
        public List<CLife> Spawns { get; }

        public CMobPool()
        {
            Spawns = new List<CLife>();
        }
        /*
        public void Load(int mapId, WzManager wzMan)
        {
            var wz = wzMan["Map.wz"];
            var path = $"Map/Map{mapId / 100000000}/{mapId}.img/life";
            var life = wz.ResolvePath(path);

            foreach (WZObject node in life)
            {
                var type = node["type"].ValueOrDie<string>();

                if (type != "m")
                    continue;

                var id = node["id"].ValueOrDie<string>();

                var fh = node["fh"].ValueOrDie<int>();
                var x = node["x"].ValueOrDie<int>();
                var y = node["y"].ValueOrDie<int>();

                var cy = node["cy"].ValueOrDie<int>();
                var f = node["f"].ValueOrDie<int>();
                var hide = node["hide"].ValueOrDie<int>();
                var rx0 = node["rx0"].ValueOrDie<int>();
                var rx1 = node["rx1"].ValueOrDie<int>();
                var mobTime = node["mobTime"].ValueOrDie<int>();

                var cl = new CLife
                {
                    Id = Convert.ToInt32(id),
                    Type = type,
                    Foothold = fh,
                    X = x,
                    Y = y,
                    Cy = cy,
                    F = f,
                    Hide = hide,
                    Rx0 = rx0,
                    Rx1 = rx1,
                    MobTime = mobTime
                };

                Spawns.Add(cl);
            }
        }
        */

        public void Load(WZProperty mapNode)
        {
            var life = mapNode.Resolve("life").Children;

            foreach (WZProperty x in life)
            {
                var cl = new CLife();
                foreach (var portalChildNode in x.Children)
                {
                    if (portalChildNode.Name == "type")
                        cl.Type = portalChildNode.ResolveForOrNull<string>();
                    else if (portalChildNode.Name == "id")
                        cl.Id = portalChildNode.ResolveFor<int>() ?? 0;
                    else if (portalChildNode.Name == "fh")
                        cl.Foothold = portalChildNode.ResolveFor<int>() ?? 0;
                    else if (portalChildNode.Name == "x")
                        cl.X = portalChildNode.ResolveFor<int>() ?? 0;
                    else if (portalChildNode.Name == "y")
                        cl.Y = portalChildNode.ResolveFor<int>() ?? 0;
                    else if (portalChildNode.Name == "cy")
                        cl.Cy = portalChildNode.ResolveFor<int>() ?? 0;
                    else if (portalChildNode.Name == "rx0")
                        cl.Rx0 = portalChildNode.ResolveFor<int>() ?? 0;
                    else if (portalChildNode.Name == "rx1")
                        cl.Rx1 = portalChildNode.ResolveFor<int>() ?? 0;
                    else if (portalChildNode.Name == "f")
                        cl.F = portalChildNode.ResolveFor<int>() ?? 0;
                    else if (portalChildNode.Name == "hide")
                        cl.Hide = portalChildNode.ResolveFor<int>() ?? 0;
                    else if (portalChildNode.Name == "mobTime")
                        cl.MobTime = portalChildNode.ResolveFor<int>() ?? 0;
                    else if (portalChildNode.Name == "f")
                        cl.F = portalChildNode.ResolveFor<int>() ?? 0;
                }

                if (cl.Type != "m")
                    continue;

                Spawns.Add(cl);
            }
        }
        
        public void DoMobLogic()
        {
            foreach (var spawn in Spawns)
            {
                var mob = new CMob(spawn.Id)
                {
                    dwMobId = GetUniqueId()
                };

                mob.Position.Position.X = (short)spawn.X;
                mob.Position.Position.Y = (short)spawn.Cy;
                mob.Position.Foothold = (short)spawn.Foothold;

                Add(mob.dwMobId, mob);}
        }
    }
}
