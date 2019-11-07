using System.Collections.Generic;

namespace RoadTrip.Game
{
    public class Codex
    {
        public List<TerrainType> TerrainTypes { get; set; } = new List<TerrainType>();

        public Dictionary<string, TerrainType> TerrainLookup { get; set; } = new Dictionary<string, TerrainType>();
    }
}
