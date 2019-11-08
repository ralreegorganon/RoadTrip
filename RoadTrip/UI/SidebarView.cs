using System.Drawing;
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

            Title = "Sidebar";
        }

        private EcsWorld World { get; }

        private Game.Game Game { get; }

        public override void Draw()
        {
            base.Draw();

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

            Print(0, 1, ContentAlignment.TopLeft, $"[font=text]Location: {terrain.Name}[/font]");
        }
    }
}
