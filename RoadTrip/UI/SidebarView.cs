using System.Drawing;
using BearLib;
using Leopotam.Ecs;
using RoadTrip.Game;
using RoadTrip.Game.Components;

namespace RoadTrip.UI
{
    public class SidebarView : View
    {
        public SidebarView(Game.Game game, EcsWorld world)
        {
            Game = game;
            World = world;
        }

        private EcsWorld World { get; }

        private Game.Game Game { get; }

        public override void Draw(RunState currentRunState)
        {
            base.Draw(currentRunState);
            Terminal.BkColor(Color.CornflowerBlue);
            Terminal.Color(Color.White);
            for (var x = 0; x < ScreenFrameAbs.Width; x+=SpacingX) {
                Print(x, 0, ContentAlignment.TopLeft, "x");
            }

            Print(0, SpacingY, ContentAlignment.TopLeft, "0123456789ABCDEFGHIJKLMNO");


            Terminal.Layer(1);
            Terminal.BkColor(Color.Black);
            Terminal.Color(Color.White);

            var cameraFocusFilter = World.GetFilter(typeof(EcsFilter<Position, CameraFocusTag>));

            if (cameraFocusFilter.IsEmpty())
            {
                return;
            }

            ref var cameraFocus = ref cameraFocusFilter.Entities[0];
            var cameraFocusPosition = cameraFocus.Get<Position>();

            var mapMemory = Game.Player.Get<MapMemory>();

            var c = cameraFocusPosition.Coordinate;

            var remembered = mapMemory.Remembered.Contains(c);
            if (!remembered)
            {
                return;
            }

            if (!Game.Map.Terrain.TryGetValue(c, out var terrain))
            {
                return;
            }

            Print(0, SpacingY*2, ContentAlignment.TopLeft, $"Location: {terrain.Name}");
        }
    }
}
