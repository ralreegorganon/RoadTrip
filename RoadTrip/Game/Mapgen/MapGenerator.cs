using System;

namespace RoadTrip.Game.Mapgen
{
    public class MapGenerator
    {
        public Codex Codex { get; set; }

        public MapGenerator(Codex codex)
        {
            Codex = codex;
        }

        public Map Generate()
        {
            Random r = new Random();

            var map = new Map();

            var wall = Codex.TerrainLookup["t_wall"];
            var floor = Codex.TerrainLookup["t_floor"];

            var z = 0;
            for (var x = 0; x < 32; x++)
            {
                for(var y = 0; y < 32; y++)
                {
                    if (x == 0 || x == 31 || y == 0 || y == 31) {
                        map.Terrain[new Coordinate(x, y, z)] = wall;
                    }
                    else {
                        map.Terrain[new Coordinate(x, y, z)] = floor;
                    }
                }
            }

            for (var x = 15; x < 17; x++)
            {
                for (var y = 15; y < 17; y++)
                {
                    map.Terrain[new Coordinate(x, y, z)] = wall;
                }
            }

            for (var i = 0; i < 100; i++) {
                map.Terrain[new Coordinate(r.Next(1, 30), r.Next(1, 30), z)] = wall;
            }

            return map;
        }
    }
}
