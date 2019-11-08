using System;
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

        private EcsWorld World { get; }

        private Game.Game Game { get; }

        public override void Draw(RunState currentRunState)
        {
            base.Draw(currentRunState);

            var cameraFocusFilter = World.GetFilter(typeof(EcsFilter<Position, CameraFocusTag>));

            if (cameraFocusFilter.IsEmpty()) {
                return;
            }

            ref var cameraFocus = ref cameraFocusFilter.Entities[0];
            var cameraFocusPosition = cameraFocus.Get<Position>();

            var viewshed = Game.Player.Get<Viewshed>();
            var mapMemory = Game.Player.Get<MapMemory>();

            var worldFrameAbs = new Rectangle(cameraFocusPosition.Coordinate.X - ScreenFrameAbs.Width / 2, cameraFocusPosition.Coordinate.Y - ScreenFrameAbs.Height / 2, ScreenFrameAbs.Width, ScreenFrameAbs.Height);

            for (var sx = 0; sx < ScreenFrameAbs.Width; sx++)
            for (var sy = 0; sy < ScreenFrameAbs.Height; sy++) {
                var wp = Point.Add(worldFrameAbs.Location, new Size(sx, sy));
                var c = new Coordinate(wp.X, wp.Y, cameraFocusPosition.Coordinate.Z);

                // We never forget and anything visible is in memory...
                var remembered = mapMemory.Remembered.Contains(c);
                if (!remembered) {
                    continue;
                }

                // If there's nothing to draw here... don't.
                if (!Game.Map.Terrain.TryGetValue(c, out var terrain)) {
                    continue;
                }

                var visible = viewshed.Visible.Contains(c);
                if (visible) {
                    Terminal.Color(terrain.Renderable.FgColor);
                }
                else {
                    var luminosity = ((0.21 * terrain.Renderable.FgColor.R) + (0.72 * terrain.Renderable.FgColor.G) + (0.07 * terrain.Renderable.FgColor.B)) / 3;
                    var gray = Convert.ToInt32(luminosity);
                    var newColor = Color.FromArgb(255, gray, gray, gray);
                    Terminal.Color(newColor);
                }

                Put(sx, sy, terrain.Renderable.Symbol);
            }

            var renderable = World.GetFilter(typeof(EcsFilter<Position, Renderable>));
            foreach (var i in renderable) {
                ref var e = ref renderable.Entities[i];
                var pos = e.Get<Position>();
                var render = e.Get<Renderable>();

                var screenPos = WorldToScreen(pos.Coordinate, worldFrameAbs, cameraFocusPosition.Coordinate);
                if (screenPos != null) {
                    Terminal.Color(render.FgColor);
                    Put(screenPos.Value.X, screenPos.Value.Y, render.Symbol);
                }
            }

            if (CurrentRunState == RunState.ShowTargeting) {
                var from = Game.Player.Get<Position>();
                var to = Game.Cursor.Get<Position>();

                if (false) {
                    var bpath = Bresenham.Line(from.Coordinate, to.Coordinate)
                        .Skip(1);
                    Terminal.Composition(true);
                    foreach (var c in bpath)
                    {
                        var screenPos = WorldToScreen(c, worldFrameAbs, cameraFocusPosition.Coordinate);
                        if (screenPos != null)
                        {
                            Terminal.Color(Color.FromArgb(128, 255, 0, 0));
                            Put(screenPos.Value.X, screenPos.Value.Y, '█');

                            if (c == to.Coordinate)
                            {
                                Terminal.Color(Color.Red);
                                Put(screenPos.Value.X, screenPos.Value.Y, '▔');
                                Put(screenPos.Value.X, screenPos.Value.Y, '▁');
                                Put(screenPos.Value.X, screenPos.Value.Y, '▏');
                                Put(screenPos.Value.X, screenPos.Value.Y, '▕');
                            }
                        }
                    }
                    Terminal.Composition(false);
                }
                else {
                    var xpath = XiaolinWu.Line(from.Coordinate, to.Coordinate).Where(x => {
                        if (x.C == to.Coordinate) {
                            return true;
                        }

                        var hasChance = x.A > 0;
                        if (!hasChance) {
                            return false;
                        }

                        return viewshed.Visible.Contains(x.C);
                    }).Skip(1).ToList();

                    var color = viewshed.Visible.Contains(to.Coordinate) ? Color.LawnGreen : Color.Red;

                    Terminal.Composition(true);
                    foreach (var c in xpath)
                    {
                        var screenPos = WorldToScreen(c.C, worldFrameAbs, cameraFocusPosition.Coordinate);
                        if (screenPos != null) {
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
