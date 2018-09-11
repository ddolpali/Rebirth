using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Server;
using PKG1;

namespace Common.Game
{
    public class CPortalMan
    {
        public List<Portal> Portals { get; }

        public CPortalMan()
        {
            Portals = new List<Portal>();
        }
        /*
        public void Load(int mapId,WzManager wzMan)
        {
            var wz = wzMan["Map.wz"];
            var path = $"Map/Map{mapId / 100000000}/{mapId}.img/portal";
            var portals = wz.ResolvePath(path);

            foreach (WZObject x in portals)
            {
                var p = new Portal
                {
                    nIdx = Convert.ToInt32(x.Name),
                    sName = x["pn"].ValueOrDie<string>(),
                    nType = x["pt"].ValueOrDie<int>(),
                    nTMap = x["tm"].ValueOrDie<int>(),
                    sTName = x["tn"].ValueOrDie<string>(),
                    ptPos =
                    {
                        X = (short)x["x"].ValueOrDie<int>(),
                        Y = (short)x["y"].ValueOrDie<int>()
                    }
                };

                Portals.Add(p);
            }
        }*/
        public void Load(WZProperty mapNode)
        {
            var portals = mapNode.Resolve("portal").Children;

            foreach (WZProperty x in portals)
            {
                var p = new Portal();
                p.nIdx = Convert.ToInt32(x.Name);
                foreach (var portalChildNode in x.Children)
                {
                    if (portalChildNode.Name == "pn")
                        p.sName = portalChildNode.ResolveForOrNull<string>();
                    else if (portalChildNode.Name == "pt")
                        p.nType = portalChildNode.ResolveFor<int>() ?? 0;
                    else if (portalChildNode.Name == "tm")
                        p.nTMap = portalChildNode.ResolveFor<int>() ?? 0;
                    else if (portalChildNode.Name == "tn")
                        p.sTName = portalChildNode.ResolveForOrNull<string>();
                    else if (portalChildNode.Name == "x")
                        p.ptPos.X = (short)(portalChildNode.ResolveFor<int>() ?? 0);
                    else if (portalChildNode.Name == "y")
                        p.ptPos.Y = (short)(portalChildNode.ResolveFor<int>() ?? 0);
                }

                Portals.Add(p);
            }
        }

        public Portal GetByName(string name) => Portals.FirstOrDefault(p => p.sName == name);

        public byte GetRandomSpawn()
        {
            var list = Portals.Where(p => p.sName == "sp").ToArray();

            if (list.Length == 0)
                return 0;

            return (byte)list.Random().nIdx;
        }
    }
}
