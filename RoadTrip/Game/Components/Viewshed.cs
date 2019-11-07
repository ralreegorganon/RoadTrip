using System.Collections.Generic;

namespace RoadTrip.Game.Components
{
    public class Viewshed
    {
        public HashSet<Coordinate> Visible { get; set; } = new HashSet<Coordinate>();
        public int Range { get; set; }
        public bool Dirty { get; set; }
    }
}
