using System.Drawing;
using BearLib;
using Leopotam.Ecs;
using RoadTrip.Game;
using RoadTrip.Game.Components;

namespace RoadTrip.UI
{
    public class TargetingView : View
    {
        public TargetingView(Game.Game game, EcsWorld world)
        {
            Game = game;
            World = world;
        }

        private EcsWorld World { get; }

        private Game.Game Game { get; }

        public override void Draw(RunState currentRunState)
        {
            Terminal.Color(Color.White);

            if (currentRunState != RunState.ShowTargeting) {
                return;
            }

            base.Draw(currentRunState);

            var mapMemory = Game.Player.Get<MapMemory>();
            var cursorPosition = Game.Cursor.Get<Position>();
            var playerPosition = Game.Player.Get<Position>();

            var c = cursorPosition.Coordinate;
            var p = playerPosition.Coordinate;

            var remembered = mapMemory.Remembered.Contains(c);
            if (!remembered)
            {
                return;
            }

            if (!Game.Map.Terrain.TryGetValue(c, out var terrain))
            {
                return;
            }

            var distance = Coordinate.ChebyshevDistance(p, c);

            Print(0, 1, ContentAlignment.TopLeft, $"[[{distance}]] {terrain.Name}");
        }
    }
}
