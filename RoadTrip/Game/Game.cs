using System.Collections.Generic;
using Leopotam.Ecs;
using RoadTrip.Game.Mapgen;
using RoadTrip.Game.Systems;
using Serilog;

namespace RoadTrip.Game
{
    public class Game
    {
        public Game(EcsWorld world, EcsSystems systems, MapGenerator mapGenerator, ILogger logger)
        {
            World = world;
            Systems = systems;
            Logger = logger;
            MapGenerator = mapGenerator;
        }

        public bool Run { get; set; } = true;

        public Map Map { get; set; } = new Map();

        public MapGenerator MapGenerator { get; set; }

        public EcsEntity Player { get; set; }

        public EcsEntity Cursor { get; set; }

        private EcsWorld World { get; }

        private EcsSystems Systems { get; }

        private ILogger Logger { get; }

        public void Setup()
        {
            Map = MapGenerator.Generate();
        }

        public void Tick()
        {
            Systems.Run();
            World.EndFrame();
        }

        private List<string> GameplaySystems { get; } = new List<string> {
            nameof(MapIndexingSystem),
            nameof(VisibilitySystem)
        };

        public void EnableGameplaySystems()
        {
            foreach (var s in GameplaySystems) {
                var idx = Systems.GetNamedRunSystem(s);
                if (!Systems.GetRunSystemState(idx)) {
                    Systems.SetRunSystemState(idx, true);
                }
            }
        }

        public void DisableGameplaySystems()
        {
            foreach (var s in GameplaySystems) {
                var idx = Systems.GetNamedRunSystem(s);
                if (Systems.GetRunSystemState(idx)) {
                    Systems.SetRunSystemState(idx, false);
                }
            }
        }
    }
}
