using BenchmarkDotNet.Attributes;
using DryIoc;
using RoadTrip;
using RoadTrip.Game;
using RoadTrip.Game.Components;

namespace RoadTripBenchmark
{
    public abstract class BaselineBenchmark
    {
        protected Game Game { get; set; }

        [GlobalSetup]
        public virtual void GlobalSetup()
        {
            var god = new God();
            god.Container.Resolve<ScriptLoader>();
            Game = god.Container.Resolve<Game>();
            Game.Setup();
        }
    }

    public class DumbBenchmark : BaselineBenchmark
    {
        [Benchmark]
        public void DoIt()
        {
            var mcr = Game.Player.Set<WantsToMove>();
            mcr.Movement = new Coordinate(1, 0, 0);
            Game.Tick();
        }
    }
}
