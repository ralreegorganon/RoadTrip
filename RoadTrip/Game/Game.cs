using Leopotam.Ecs;
using RoadTrip.Game.Mapgen;
using Serilog;

namespace RoadTrip.Game
{
    public class Game
    {
        public bool Run { get; set; } = true;

        public Map Map { get; set; } = new Map();

        public MapGenerator MapGenerator { get; set; }

        public EcsEntity Player { get; set; }

        public EcsEntity Cursor { get; set; }

        private EcsWorld World { get; }

        private EcsSystems Systems { get; }

        private ILogger Logger { get; }

        public Game(EcsWorld world, EcsSystems systems, MapGenerator mapGenerator, ILogger logger)
        {
            World = world;
            Systems = systems;
            Logger = logger;
            MapGenerator = mapGenerator;
        }

        public void Setup()
        {
            Map = MapGenerator.Generate();
        }

        public void Tick()
        {
            Systems.Run();
            World.EndFrame();
        }
    }
}
