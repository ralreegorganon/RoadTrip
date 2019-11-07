using System.Collections.Generic;

namespace RoadTrip.Game.Components
{
    public class MapMemory
    {
        public HashSet<Coordinate> Remembered { get; set; } = new HashSet<Coordinate>();
    }
}
