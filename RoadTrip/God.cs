using System;
using DryIoc;
using Leopotam.Ecs;
using RoadTrip.Game;
using RoadTrip.Game.Systems;
using RoadTrip.UI;

namespace RoadTrip
{
    public class God
    {
        public Container Container { get; }

        public God()
        {
            Container = new Container();

            Container.Register(Made.Of(() => Serilog.Log.Logger), setup: Setup.With(condition: r => r.Parent.ImplementationType == null));
            Container.Register(Made.Of(() => Serilog.Log.ForContext(Arg.Index<Type>(0)), r => r.Parent.ImplementationType), setup: Setup.With(condition: r => r.Parent.ImplementationType != null));

            Container.Register<Codex>(Reuse.Singleton);
            Container.Register<Game.Game>(Reuse.Singleton);

            Container.Register<RootView>(Reuse.Singleton);
            Container.Register<MapView>(Reuse.Singleton);
            Container.Register<SidebarView>(Reuse.Singleton);

            Container.Register<EcsWorld>(Reuse.Singleton);
            Container.Register<EcsSystems>(Reuse.Singleton);

            Container.Register<DummyFilterSystem>(Reuse.Singleton);
            Container.Register<MoveSystem>(Reuse.Singleton);
            Container.Register<CursorSystem>(Reuse.Singleton);

            var systems = Container.Resolve<EcsSystems>();
            systems.Add(Container.Resolve<DummyFilterSystem>());
            systems.Add(Container.Resolve<MoveSystem>());
            systems.Add(Container.Resolve<CursorSystem>());

            systems.Init();
        }
    }
}
