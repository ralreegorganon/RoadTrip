using RoadTrip.Game.Components;

namespace RoadTrip.Game
{
    public class TerrainType
    {
        public string Name { get; set; }
        public Renderable Renderable { get; set; }
        public bool IsOpaque { get; set; }
    }
}
