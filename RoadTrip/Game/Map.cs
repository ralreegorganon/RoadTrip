using System.Collections.Generic;

namespace RoadTrip.Game
{
    public class Map
    {
        public Dictionary<Coordinate, Terrain> Terrain { get; set; } = new Dictionary<Coordinate, Terrain>();
    }
}