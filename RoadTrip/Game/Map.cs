using System.Collections.Generic;

namespace RoadTrip.Game
{
    public class Map
    {
        public Dictionary<Coordinate, TerrainType> Terrain { get; set; } = new Dictionary<Coordinate, TerrainType>();
    }
}
