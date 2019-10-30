using System.Drawing;
using BearLib;
using Leopotam.Ecs;
using RoadTrip.Game;
using RoadTrip.Game.Components;

namespace RoadTrip.UI
{
    public class MapView : View
    {
        private EcsWorld World { get; }

        private Game.Game Game { get; }

        public MapView(Game.Game game, EcsWorld world)
        {
            Game = game;
            World = world;

            BorderStyle = new BorderStyle {
                NW = '╔',
                SW = '╚',
                NE = '╗',
                SE = '╝',
                N = '═',
                S = '═',
                W = '║',
                E = '║'
            };

            Title = "Map";
        }

        public override void Draw()
        {
            base.Draw();

            var cameraFocusFilter = World.GetFilter(typeof(EcsFilter<Position, CameraFocusTag>));

            if (cameraFocusFilter.IsEmpty()) {
                return;
            }

            ref var cameraFocus = ref cameraFocusFilter.Entities[0];
            var cameraFocusPosition = cameraFocus.Get<Position>();

            var worldFrameAbs = new Rectangle(cameraFocusPosition.Coordinate.X - ScreenFrameAbs.Width / 2, cameraFocusPosition.Coordinate.Y - ScreenFrameAbs.Height / 2, ScreenFrameAbs.Width, ScreenFrameAbs.Height);

            for (var sx = 0; sx < ScreenFrameAbs.Width; sx++)
            for (var sy = 0; sy < ScreenFrameAbs.Height; sy++) {
                var wp = Point.Add(worldFrameAbs.Location, new Size(sx, sy));
                var c = new Coordinate(wp.X, wp.Y, cameraFocusPosition.Coordinate.Z);
                if (Game.Map.Terrain.TryGetValue(c, out var terrain))
                {
                    Terminal.Color(terrain.Renderable.Color);
                    Put(sx, sy, terrain.Renderable.Symbol);
                }
            }

            var renderable = World.GetFilter(typeof(EcsFilter<Position, Renderable>));
            foreach (var i in renderable) {
                ref var e = ref renderable.Entities[i];
                var pos = e.Get<Position>();
                var render = e.Get<Renderable>();

                var screenPos = WorldToScreen(pos.Coordinate, worldFrameAbs, cameraFocusPosition.Coordinate);
                if (screenPos != null) {
                    Terminal.Color(render.Color);
                    Put(screenPos.Value.X, screenPos.Value.Y, '@');
                }
            }
        }

        public static Coordinate? WorldToScreen(Coordinate world, Rectangle worldFrameAbs, Coordinate cameraFocus)
        {
            if (world.Z != cameraFocus.Z) {
                return null;
            }

            if (!worldFrameAbs.Contains(world.X, world.Y)) {
                return null;
            }

            return new Coordinate(world.X - worldFrameAbs.Location.X, world.Y - worldFrameAbs.Location.Y, world.Z);
        }
    }
}