using System.Drawing;
using RoadTrip.Game.Components;

namespace RoadTrip.Game
{
    public class TerrainType
    {
        public TerrainType()
        {
        }

        public TerrainType(string id, string name, char sym, Color fg, Color bg, bool isOpaque) : base()
        {
            Id = id;
            Name = name;
            Renderable.Symbol = sym;
            Renderable.FgColor = fg;
            Renderable.BgColor = bg;
            IsOpaque = isOpaque;
        }

        public string Id { get; set; } = "Unknown";
        public string Name { get; set; } = "Unknown";
        public Renderable Renderable { get; set; } = new Renderable {Symbol = '?', FgColor = Color.White, BgColor = Color.Black };
        public bool IsOpaque { get; set; }
    }
}
