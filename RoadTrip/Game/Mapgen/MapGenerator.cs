using System;

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
            var r = new Random();

            var map = new Map();

            var wall = Codex.TerrainLookup["t_wall"];
            var floor = Codex.TerrainLookup["t_floor"];

            var z = 0;
            for (var x = 0; x < 256; x++) {
                for (var y = 0; y < 256; y++) {
                    if (x == 0 || x == 255 || y == 0 || y == 255) {
                        map.Terrain[new Coordinate(x, y, z)] = wall;
                    }
                    else {
                        map.Terrain[new Coordinate(x, y, z)] = floor;
                    }
                }
            }

            for (var i = 0; i < 10000; i++) {
                map.Terrain[new Coordinate(r.Next(1, 255), r.Next(1, 255), z)] = wall;
            }

            return map;
        }
    }
}
