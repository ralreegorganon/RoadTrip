using Leopotam.Ecs;
using RoadTrip.Game.Components;

namespace RoadTrip.Game.Systems
{
    public class MoveSystem : IEcsRunSystem
    {
        private EcsFilter<Position, WantsToMove> Filter { get; set; }

        public void Run()
        {
            foreach (var i in Filter)
            {
                ref var entity = ref Filter.Entities[i];
                var move = entity.Get<WantsToMove>();
                var position = entity.Get<Position>();

                position.Coordinate += move.Movement;

                entity.Unset<WantsToMove>();
            }
        }
    }
}
