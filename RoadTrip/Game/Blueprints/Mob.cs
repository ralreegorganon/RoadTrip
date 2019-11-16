using System.Drawing;

namespace RoadTrip.Game.Blueprints
{
    public class RenderableBlueprint
    {
        public char Symbol { get; set; }
        public Color Color { get; set; }
    }

    public class MobBlueprint
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public RenderableBlueprint Renderable { get; set; }
    }
}
