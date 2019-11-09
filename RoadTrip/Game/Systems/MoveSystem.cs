using Leopotam.Ecs;
using RoadTrip.Game.Components;

namespace RoadTrip.Game.Systems
{
    public class MoveSystem : IEcsRunSystem
    {
        public MoveSystem(Game game)
        {
            Game = game;
        }

        private Game Game { get; }

        private EcsFilter<Position, WantsToMove>? Filter { get; set; }

        public void Run()
        {
            foreach (var i in Filter!) {
                ref var entity = ref Filter.Entities[i];
                var move = entity.Get<WantsToMove>();
                var position = entity.Get<Position>();

                var desired = position.Coordinate + move.Movement;

                if (entity.Get<CursorTag>() != null) {
                    position.Coordinate = desired;
                } else if (Game.Map.Terrain.TryGetValue(desired, out var terrain)) {
                    if (!terrain.IsOpaque || entity.Get<IncorporealTag>() != null) {
                        position.Coordinate = desired;

                        var viewshed = entity.Get<Viewshed>();
                        if (viewshed != null) {
                            viewshed.Dirty = true;
                        }
                    }
                }

                entity.Unset<WantsToMove>();
            }
        }
    }
}
