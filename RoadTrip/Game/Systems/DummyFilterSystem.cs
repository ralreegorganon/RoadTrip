using Leopotam.Ecs;
using RoadTrip.Game.Components;

namespace RoadTrip.Game.Systems
{
    public class DummyFilterSystem : IEcsSystem
    {
        private EcsFilter<Position, Renderable> RenderablePositionFilter { get; set; }

        private EcsFilter<Position, CameraFocusTag> FocusFilter { get; set; }
    }
}
