using System.Drawing;
using System.Linq;
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

            Print(0, 0, ContentAlignment.TopLeft, $"[[{distance}]] {terrain.Name}");

            if (Game.Map.TileEntities.TryGetValue(c, out var entities)) {
                var names = entities.Select(x => x.Get<Nameable>()
                        ?.Name)
                    .Where(x => x != null)
                    .ToList();

                var y = SpacingY;
                foreach (var name in names) {
                    Print(0, y, ContentAlignment.TopLeft, name);
                    y += SpacingY;
                }
            }
        }
    }
}
