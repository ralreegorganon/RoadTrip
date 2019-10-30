using DryIoc;
using Leopotam.Ecs;
using Microsoft.CodeAnalysis.CSharp.Scripting;
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
            Container.Register<Game.Game>(Reuse.Singleton);
            Container.Register<RootView>(Reuse.Singleton);
            Container.Register<MapView>(Reuse.Singleton);
            Container.Register<SidebarView>(Reuse.Singleton);
            Container.Register<EcsWorld>(Reuse.Singleton);
            Container.Register<EcsSystems>(Reuse.Singleton);
            Container.Register<DummyFilterSystem>(Reuse.Singleton);
            Container.Register<MoveSystem>(Reuse.Singleton);

            var systems = Container.Resolve<EcsSystems>();
            systems.Add(Container.Resolve<DummyFilterSystem>());
            systems.Add(Container.Resolve<MoveSystem>());
            systems.Init();
        }
    }
}
