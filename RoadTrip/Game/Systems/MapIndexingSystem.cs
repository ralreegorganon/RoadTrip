using System;
using System.Collections.Generic;
using System.Text;
using Leopotam.Ecs;
using RoadTrip.Game.Components;

namespace RoadTrip.Game.Systems
{
    public class MapIndexingSystem : IEcsRunSystem
    {
        public MapIndexingSystem(Game game)
        {
            Game = game;
        }

        private Game Game { get; }

        private EcsFilter<Position>? Filter { get; set; }

        public void Run()
        {
            Game.Map.TileEntities.Clear();

            foreach (var i in Filter!)
            {
                ref var entity = ref Filter.Entities[i];
                var position = entity.Get<Position>();

                if (Game.Map.TileEntities.TryGetValue(position.Coordinate, out var entities)) {
                    entities.Add(entity);
                }
                else {
                    Game.Map.TileEntities[position.Coordinate] = new List<EcsEntity> {entity};
                }
            }
        }
    }
}
