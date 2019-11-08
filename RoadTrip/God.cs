﻿using System;
using DryIoc;
using Leopotam.Ecs;
using RoadTrip.Game;
using RoadTrip.Game.Mapgen;
using RoadTrip.Game.Systems;
using RoadTrip.UI;
using Serilog;

namespace RoadTrip
{
    public class God
    {
        public God()
        {
            Container = new Container();

            Container.Register(Made.Of(() => Log.Logger), setup: Setup.With(condition: r => r.Parent.ImplementationType == null));
            Container.Register(Made.Of(() => Log.ForContext(Arg.Index<Type>(0)), r => r.Parent.ImplementationType), setup: Setup.With(condition: r => r.Parent.ImplementationType != null));

            Container.Register<ScriptLoader>(Reuse.Singleton);
            Container.Register<Codex>(Reuse.Singleton);
            Container.Register<Game.Game>(Reuse.Singleton);
            Container.Register<MapGenerator>(Reuse.Singleton);

            Container.Register<InputResolver>(Reuse.Singleton);

            Container.Register<RootView>(Reuse.Singleton);
            Container.Register<MapView>(Reuse.Singleton);
            Container.Register<SidebarView>(Reuse.Singleton);

            Container.Register<EcsWorld>(Reuse.Singleton);
            Container.Register<EcsSystems>(Reuse.Singleton);

            Container.Register<DummyFilterSystem>(Reuse.Singleton);
            Container.Register<MoveSystem>(Reuse.Singleton);
            Container.Register<CursorSystem>(Reuse.Singleton);
            Container.Register<VisibilitySystem>(Reuse.Singleton);

            var systems = Container.Resolve<EcsSystems>();
            systems.Add(Container.Resolve<DummyFilterSystem>());
            systems.Add(Container.Resolve<VisibilitySystem>(), nameof(VisibilitySystem));
            systems.Add(Container.Resolve<MoveSystem>(), nameof(MoveSystem));
            systems.Add(Container.Resolve<CursorSystem>(), nameof(CursorSystem));

            systems.Init();
        }

        public Container Container { get; }
    }
}
