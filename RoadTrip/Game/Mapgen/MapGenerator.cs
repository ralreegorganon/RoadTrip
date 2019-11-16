using System;
using System.Linq;

namespace RoadTrip.Game.Mapgen
{
    public class MapGenerator
    {
        public MapGenerator(Codex codex)
        {
            Codex = codex;
        }

        public Codex Codex { get; set; }

        public Map Generate()
        {
            return Drunk();
        }

        private Map Classic()
        {
            var r = new Random();

            var map = new Map();

            var wall = Codex.TerrainLookup["t_wall"];
            var floor = Codex.TerrainLookup["t_floor"];

            var z = 0;
            for (var x = -1; x < 255; x++)
            {
                for (var y = -1; y < 255; y++)
                {
                    if (x == -1 || x == 254 || y == -1 || y == 254)
                    {
                        map.Terrain[new Coordinate(x, y, z)] = wall;
                    }
                    else
                    {
                        map.Terrain[new Coordinate(x, y, z)] = floor;
                    }
                }
            }

            for (var i = 0; i < 10000; i++)
            {
                map.Terrain[new Coordinate(r.Next(-1, 254), r.Next(-1, 254), z)] = wall;
            }

            return map;
        }

        private Map Drunk()
        {
            var r = new Random();

            var map = new Map();

            var wall = Codex.TerrainLookup["t_wall"];
            var floor = Codex.TerrainLookup["t_floor"];

            for (var i = 0; i < 10; i++) {
                var loc = new Coordinate(0, 0, 0);

                map.Terrain[loc] = floor;

                var c = 0;
                while (c < 256)
                {
                    var n = r.Next(0, 3);
                    switch (n)
                    {
                        case 0:
                            loc += new Coordinate(1, 0, 0);
                            break;
                        case 1:
                            loc += new Coordinate(-1, 0, 0);
                            break;
                        case 2:
                            loc += new Coordinate(0, 1, 0);
                            break;
                        case 3:
                            loc += new Coordinate(0, -1, 0);
                            break;
                    }

                    if (map.Terrain.ContainsKey(loc))
                    {
                        continue;
                    }

                    map.Terrain[loc] = floor;
                    c++;
                }
            }

            var maxX = map.Terrain.Keys.Max(x => x.X);
            var maxY = map.Terrain.Keys.Max(x => x.Y);
            var minX = map.Terrain.Keys.Min(x => x.X);
            var minY = map.Terrain.Keys.Min(x => x.Y);

            for (var x = minX; x <= maxX; x++) {
                for (var y = minY; y <= maxY; y++) {
                    var c = new Coordinate(x, y, 0);
                    if (!map.Terrain.ContainsKey(c)) {
                        map.Terrain[c] = wall;
                    }
                }
            }

            return map;
        }
    }
}
