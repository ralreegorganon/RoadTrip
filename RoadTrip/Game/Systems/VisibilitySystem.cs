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
                viewshed.Visible = DeriveViewshed(position.Coordinate, Game.Map);
            }
        }

        private HashSet<Coordinate> DeriveViewshed(Coordinate coordinate, Map map)
        {
            var viewshed = new HashSet<Coordinate>();

            for (var dx = -8; dx <= 8; dx++) {
                for (var dy = -8; dy <= 8; dy++) {
                    viewshed.Add(coordinate + new Coordinate(dx, dy, 0));
                }
            }

            return viewshed;
        }
    }
}
