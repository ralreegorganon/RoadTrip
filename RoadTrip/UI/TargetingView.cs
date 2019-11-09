using System.Drawing;
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

            BorderStyle = new BorderStyle {
                NW = '╒',
                SW = '╘',
                NE = '╕',
                SE = '╛',
                N = '╌',
                S = '╌',
                W = '╎',
                E = '╎'
            };

            Title = "Target";
        }

        private EcsWorld World { get; }

        private Game.Game Game { get; }

        public override void Draw(RunState currentRunState)
        {
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

            Print(0, 1, ContentAlignment.TopLeft, $"[font=text][[{distance}]] {terrain.Name}[/font]]");
        }
    }
}
