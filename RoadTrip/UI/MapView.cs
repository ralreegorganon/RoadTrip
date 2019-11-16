using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BearLib;
using Leopotam.Ecs;
using RoadTrip.Game;
using RoadTrip.Game.Components;

namespace RoadTrip.UI
{
    public class MapView : View
    {
        public MapView(Game.Game game, EcsWorld world)
        {
            Game = game;
            World = world;
        }

        private EcsWorld World { get; }

        private Game.Game Game { get; }


        public static IEnumerable<(Coordinate World, Coordinate Screen)> Each(Rectangle worldFrame, Rectangle screenFrame, Coordinate cameraFocus, int xRatio, int yRatio)
        {
            for (var wx = worldFrame.X; wx < worldFrame.Right; wx++) {
                for (var wy = worldFrame.Y; wy < worldFrame.Bottom; wy++) {
                    var wc = new Coordinate(wx, wy, cameraFocus.Z);
                    var sc = WorldToScreen(wc, worldFrame, screenFrame, cameraFocus, xRatio, yRatio);
                    yield return (wc, sc.Value);
                }
            }
        }

        public override void Draw(RunState currentRunState)
        {
            base.Draw(currentRunState);

            Terminal.Font(Font);


            var cameraFocusFilter = World.GetFilter(typeof(EcsFilter<Position, CameraFocusTag>));

            if (cameraFocusFilter.IsEmpty()) {
                return;
            }

            ref var cameraFocus = ref cameraFocusFilter.Entities[0];
            var cameraFocusPosition = cameraFocus.Get<Position>();

            var viewshed = Game.Player.Get<Viewshed>();
            var mapMemory = Game.Player.Get<MapMemory>();

            // The coordinate range, in world coordinates, that are visible in the screen.
            var worldFrameAbs = new Rectangle(cameraFocusPosition.Coordinate.X - ScreenFrameAbs.Width / SpacingX / 2, cameraFocusPosition.Coordinate.Y - ScreenFrameAbs.Height / SpacingY / 2, ScreenFrameAbs.Width / SpacingX, ScreenFrameAbs.Height / SpacingY);

            foreach (var c in Each(worldFrameAbs, ScreenFrameAbs, cameraFocusPosition.Coordinate, SpacingX, SpacingY)) {
                // We never forget and anything visible is in memory...
                var remembered = mapMemory.Remembered.Contains(c.World);
                if (!remembered)
                {
                    continue;
                }

                // If there's nothing to draw here... don't.
                if (!Game.Map.Terrain.TryGetValue(c.World, out var terrain))
                {
                    continue;
                }


                var visible = viewshed.Visible.Contains(c.World);
                if (visible)
                {
                    Terminal.Color(terrain.Renderable.FgColor);
                    Terminal.BkColor(terrain.Renderable.BgColor);
                }
                else
                {
                    Terminal.Color(terrain.Renderable.FgColor.Greyscale());
                    Terminal.BkColor(terrain.Renderable.BgColor.Greyscale());
                }

                Put(c.Screen.X, c.Screen.Y, terrain.Renderable.Symbol);
            }

            Terminal.BkColor(Color.Black);

            var renderable = World.GetFilter(typeof(EcsFilter<Position, Renderable>));
            foreach (var i in renderable)
            {
                ref var e = ref renderable.Entities[i];
                var pos = e.Get<Position>();
                var render = e.Get<Renderable>();

                var screenPos = WorldToScreen(pos.Coordinate, worldFrameAbs, ScreenFrameAbs, cameraFocusPosition.Coordinate, SpacingX, SpacingY);
                if (screenPos != null)
                {
                    Terminal.Color(render.FgColor);
                    Put(screenPos.Value.X, screenPos.Value.Y, render.Symbol);
                }
            }

            if (CurrentRunState == RunState.ShowTargeting)
            {
                var from = Game.Player.Get<Position>();
                var to = Game.Cursor.Get<Position>();

                var points = true
                    ? Wu2.Line(from.Coordinate, to.Coordinate) : Bresenham.Line(from.Coordinate, to.Coordinate)
                        .Select(x => (C: x, A: 1.0));

                var xpath = points.Where(x => {
                    if (x.C == from.Coordinate)
                    {
                        return false;
                    }

                    if (x.C == to.Coordinate)
                    {
                        return true;
                    }

                    var hasChance = x.A > 0;
                    if (!hasChance)
                    {
                        return false;
                    }

                    return viewshed.Visible.Contains(x.C);
                }).ToList();

                var color = viewshed.Visible.Contains(to.Coordinate) ? Color.LawnGreen : Color.Red;

                Terminal.Composition(true);
                foreach (var c in xpath)
                {
                    var screenPos = WorldToScreen(c.C, worldFrameAbs, ScreenFrameAbs, cameraFocusPosition.Coordinate, SpacingX, SpacingY);
                    if (screenPos != null)
                    {
                        Terminal.Color(Color.FromArgb((int)(180 * c.A), color.R, color.G, color.B));
                        Put(screenPos.Value.X, screenPos.Value.Y, '█');

                        if (c.C == to.Coordinate)
                        {
                            Terminal.Color(color);
                            Put(screenPos.Value.X, screenPos.Value.Y, '▔');
                            Put(screenPos.Value.X, screenPos.Value.Y, '▁');
                            Put(screenPos.Value.X, screenPos.Value.Y, '▏');
                            Put(screenPos.Value.X, screenPos.Value.Y, '▕');
                        }
                    }
                }
                Terminal.Composition(false);
            }
        }

        public static Coordinate? WorldToScreen(Coordinate world, Rectangle worldFrameAbs, Rectangle screenFrameAbs, Coordinate cameraFocus, int xRatio, int yRatio)
        {
            if (world.Z != cameraFocus.Z) {
                return null;
            }

            var a = world.X < worldFrameAbs.X;
            var b = world.Y < worldFrameAbs.Y;
            var c = world.X > worldFrameAbs.X + worldFrameAbs.Width;
            var d = world.Y > worldFrameAbs.Y + worldFrameAbs.Height;

            if (a || b || c || d) {
                return null;
            }


            //screen 1, 0 = wf.x + 1, wf.y

            var dx = (world.X - worldFrameAbs.Location.X);
            var dy = (world.Y - worldFrameAbs.Location.Y);

            return new Coordinate((screenFrameAbs.X + dx) * xRatio, (screenFrameAbs.Y + dy) * yRatio, world.Z);
        }
    }
}
