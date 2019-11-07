using System.Collections.Generic;
using Leopotam.Ecs;
using RoadTrip.Game.Components;

namespace RoadTrip.Game.Systems
{
    public class VisibilitySystem : IEcsRunSystem
    {
        public VisibilitySystem(Game game)
        {
            Game = game;
        }

        public Game Game { get; set; }

        private EcsFilter<Position, Viewshed> Filter { get; set; }

        public void Run()
        {
            foreach (var i in Filter)
            {
                ref var entity = ref Filter.Entities[i];

                var viewshed = entity.Get<Viewshed>();
                var position = entity.Get<Position>();

                if (!viewshed.Dirty) {
                    continue;
                }

                viewshed.Dirty = false;
                viewshed.Visible.Clear();
                ShadowCaster.ComputeFieldOfViewWithShadowCasting(position.Coordinate.X, position.Coordinate.Y, viewshed.Range, (x, y) => { return false; }, (x, y) => viewshed.Visible.Add(new Coordinate(x, y, position.Coordinate.Z)) );
            }
        }
    }
}
