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
            var worldFrameAbs = GetWorldFrameAbs(cameraFocusPosition);

            RenderTerrain(worldFrameAbs, cameraFocusPosition, mapMemory, viewshed);

            RenderOther(worldFrameAbs, cameraFocusPosition, viewshed);

            if (CurrentRunState == RunState.ShowTargeting) {
                RenderTargeting(viewshed, worldFrameAbs, cameraFocusPosition);
            }
        }

        private void RenderTargeting(Viewshed viewshed, Rectangle worldFrameAbs, Position cameraFocusPosition)
        {
            var from = Game.Player.Get<Position>();
            var to = Game.Cursor.Get<Position>();

            var points = Wu2Pair.Line(from.Coordinate, to.Coordinate);

            var path = points.Aggregate(new HashSet<Coordinate>(), (list, tuple) => {
                if (tuple.First.C == from.Coordinate) {
                    return list;
                }

                if (tuple.First.C == to.Coordinate) {
                    list.Add(tuple.First.C);
                    return list;
                }

                var viewshedContainsFirst = viewshed.Visible.Contains(tuple.First.C);
                var viewshedContainsSecond = viewshed.Visible.Contains(tuple.Second.C);

                if (!viewshedContainsFirst && !viewshedContainsSecond) {
                    return list;
                }

                if (!viewshedContainsFirst) {
                    list.Add(tuple.Second.C);
                    return list;
                }

                if (!viewshedContainsSecond)
                {
                    list.Add(tuple.First.C);
                    return list;
                }

                var firstTerrainExists = Game.Map.Terrain.TryGetValue(tuple.First.C, out var firstTerrain);
                var secondTerrainExists = Game.Map.Terrain.TryGetValue(tuple.Second.C, out var secondTerrain);

                if (!firstTerrainExists && !secondTerrainExists) {
                    return list;
                }

                if (!firstTerrainExists) {
                    list.Add(tuple.Second.C);
                    return list;
                }

                if (!secondTerrainExists) {
                    list.Add(tuple.First.C);
                    return list;
                }

                if (firstTerrain.IsOpaque && secondTerrain.IsOpaque) {
                    return list;
                }

                if (firstTerrain.IsOpaque) {
                    list.Add(tuple.Second.C);
                    return list;
                }

                if (secondTerrain.IsOpaque)
                {
                    list.Add(tuple.First.C);
                    return list;
                }

                var previous = list.LastOrDefault();
                if (previous != default) {
                    var firstDistance = Coordinate.ManhattanDistance(previous, tuple.First.C);
                    var secondDistance = Coordinate.ManhattanDistance(previous, tuple.Second.C);

                    if (firstDistance < secondDistance)
                    {
                        list.Add(tuple.First.C);
                        return list;
                    }

                    if (firstDistance > secondDistance)
                    {
                        list.Add(tuple.Second.C);
                        return list;
                    }
                }

                list.Add(tuple.First.A > tuple.Second.A ? tuple.First.C : tuple.Second.C);
                return list;
            });

            var path2 = points.Aggregate(new HashSet<(Coordinate C, double A)>(), (list, tuple) => {
                if (tuple.First.C == from.Coordinate)
                {
                    return list;
                }

                list.Add(tuple.First);
                list.Add(tuple.Second);

                return list;
            });

            var color = viewshed.Visible.Contains(to.Coordinate) ? Color.LawnGreen : Color.Red;

            Terminal.Composition(true);
            foreach (var c in path2) {
                var screenPos = WorldToScreen(c.C, worldFrameAbs, ScreenFrameAbs, cameraFocusPosition.Coordinate, SpacingX, SpacingY);
                if (screenPos == null) {
                    continue;
                }

                var alpha = 25;
                if (path.Contains(c.C)) {
                    alpha = 180;
                }

                if (c.C != to.Coordinate) {
                    Terminal.Color(Color.FromArgb(alpha, color.R, color.G, color.B));
                    Put(screenPos.Value.X, screenPos.Value.Y, '█');
                    continue;
                }

                Terminal.Color(color);
                Put(screenPos.Value.X, screenPos.Value.Y, '▔');
                Put(screenPos.Value.X, screenPos.Value.Y, '▁');
                Put(screenPos.Value.X, screenPos.Value.Y, '▏');
                Put(screenPos.Value.X, screenPos.Value.Y, '▕');
            }

            Terminal.Composition(false);
        }

        private void RenderOther(Rectangle worldFrameAbs, Position cameraFocusPosition, Viewshed viewshed)
        {
            Terminal.Layer(1);

            var renderable = World.GetFilter(typeof(EcsFilter<Position, Renderable>));
            foreach (var i in renderable) {
                ref var e = ref renderable.Entities[i];
                var pos = e.Get<Position>();
                var render = e.Get<Renderable>();

                if (!viewshed.Visible.Contains(pos.Coordinate)) {
                    continue;
                }

                var screenPos = WorldToScreen(pos.Coordinate, worldFrameAbs, ScreenFrameAbs, cameraFocusPosition.Coordinate, SpacingX, SpacingY);
                if (screenPos == null) {
                    continue;
                }

                Terminal.Color(render.FgColor);
                Put(screenPos.Value.X, screenPos.Value.Y, render.Symbol);
            }
        }

        private void RenderTerrain(Rectangle worldFrameAbs, Position cameraFocusPosition, MapMemory mapMemory, Viewshed viewshed)
        {
            Terminal.Layer(0);

            foreach (var c in Each(worldFrameAbs, ScreenFrameAbs, cameraFocusPosition.Coordinate, SpacingX, SpacingY)) {
                // We never forget and anything visible is in memory...
                var remembered = mapMemory.Remembered.Contains(c.World);
                if (!remembered) {
                    continue;
                }

                // If there's nothing to draw here... don't.
                if (!Game.Map.Terrain.TryGetValue(c.World, out var terrain)) {
                    continue;
                }

                var visible = viewshed.Visible.Contains(c.World);
                if (visible) {
                    Terminal.Color(terrain.Renderable.FgColor);
                    Terminal.BkColor(terrain.Renderable.BgColor);
                }
                else {
                    Terminal.Color(terrain.Renderable.FgColor.Greyscale());
                    Terminal.BkColor(terrain.Renderable.BgColor.Greyscale());
                }

                Put(c.Screen.X, c.Screen.Y, terrain.Renderable.Symbol);
            }
        }

        private static IEnumerable<(Coordinate World, Coordinate Screen)> Each(Rectangle worldFrame, Rectangle screenFrame, Coordinate cameraFocus, int xRatio, int yRatio)
        {
            for (var wx = worldFrame.X; wx < worldFrame.Right; wx++)
            {
                for (var wy = worldFrame.Y; wy < worldFrame.Bottom; wy++)
                {
                    var wc = new Coordinate(wx, wy, cameraFocus.Z);
                    var sc = WorldToScreen(wc, worldFrame, screenFrame, cameraFocus, xRatio, yRatio);
                    yield return (wc, sc.Value);
                }
            }
        }

        private static Coordinate? WorldToScreen(Coordinate world, Rectangle worldFrameAbs, Rectangle screenFrameAbs, Coordinate cameraFocus, int xRatio, int yRatio)
        {
            if (world.Z != cameraFocus.Z)
            {
                return null;
            }

            var a = world.X < worldFrameAbs.X;
            var b = world.Y < worldFrameAbs.Y;
            var c = world.X > worldFrameAbs.X + worldFrameAbs.Width;
            var d = world.Y > worldFrameAbs.Y + worldFrameAbs.Height;

            if (a || b || c || d)
            {
                return null;
            }


            //screen 1, 0 = wf.x + 1, wf.y

            var dx = (world.X - worldFrameAbs.Location.X);
            var dy = (world.Y - worldFrameAbs.Location.Y);

            return new Coordinate((screenFrameAbs.X + dx) * xRatio, (screenFrameAbs.Y + dy) * yRatio, world.Z);
        }

        private Rectangle GetWorldFrameAbs(Position cameraFocusPosition)
        {
            var worldFrameAbs = new Rectangle(cameraFocusPosition.Coordinate.X - ScreenFrameAbs.Width / SpacingX / 2, cameraFocusPosition.Coordinate.Y - ScreenFrameAbs.Height / SpacingY / 2, ScreenFrameAbs.Width / SpacingX, ScreenFrameAbs.Height / SpacingY);
            return worldFrameAbs;
        }
    }
}
