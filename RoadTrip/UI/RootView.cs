using System;
using System.Collections.Generic;
using System.Drawing;
using BearLib;

namespace RoadTrip.UI
{
    public enum RunState
    {
        AwaitingInput,
        PlayerTurn,
        ShowTargeting
    }

    public class RootView : View
    {
        public RootView(Game.Game game, InputResolver inputResolver, MapView mapView, SidebarView sidebarView, TargetingView targetingView)
        {
            Game = game;
            InputResolver = inputResolver;
            MapView = mapView;
            SidebarView = sidebarView;
            TargetingView = targetingView;

            var cellSizeX = 4;
            var cellSizeY = 4;

            var cellWidth = 128 * cellSizeX;
            var cellHeight = 64 * cellSizeY;

            var mapFontId = "map";
            var mapFontSizeX = 24;
            var mapFontSizeY = 24;
            var mapFontSpacingX = mapFontSizeX / cellSizeX;
            var mapFontSpacingY = mapFontSizeY / cellSizeY;

            var textFontId = "";
            var textFontSizeX = 12;
            var textFontSizeY = 24;
            var textFontSpacingX = textFontSizeX / cellSizeX;
            var textFontSpacingY = textFontSizeY / cellSizeY;

            Terminal.Open();
            Terminal.Set($"window: title='road trip', resizeable=true, size={cellWidth}x{cellHeight}, cellsize={cellSizeX}x{cellSizeY};");
            Terminal.Set($"font: ./Cascadia.ttf, size={textFontSizeX}x{textFontSizeY}, spacing={textFontSpacingX}x{textFontSpacingY}");
            Terminal.Set($"{mapFontId} font: ./Cascadia.ttf, size={mapFontSizeX}x{mapFontSizeY}, spacing={mapFontSpacingX}x{mapFontSpacingY}");
            //Terminal.Set("font: ./Topaz-8.ttf, size=16x16");
            //Terminal.Set("font: ./square.ttf, size=16x16");
            //Terminal.Set("font: ./whitrabt.ttf, size=16");
            Terminal.Set("output: vsync=false;");

            MapView.Font = mapFontId;
            MapView.SpacingX = mapFontSpacingX;
            MapView.SpacingY = mapFontSpacingY;

            SidebarView.SpacingX = textFontSpacingX;
            SidebarView.SpacingY = textFontSpacingY;

            TargetingView.SpacingX = textFontSpacingX;
            TargetingView.SpacingY = textFontSpacingY;

            Resize();

            Game.Setup();
            Game.Tick();

            RunStateStack.Push(RunState.AwaitingInput);
        }
        public InputResolver InputResolver { get; set; }

        public Game.Game Game { get; set; }

        public MapView MapView { get; }

        public SidebarView SidebarView { get; }

        public TargetingView TargetingView { get; }

        private Stack<RunState> RunStateStack { get; } = new Stack<RunState>();

        public void Tick()
        {
            Terminal.Clear();

            Game.Tick();

            var currentRunState = RunStateStack.Peek();
            switch (currentRunState)
            {
                case RunState.AwaitingInput:
                    Game.DisableGameplaySystems();
                    MapView.Draw(currentRunState);
                    SidebarView.Draw(currentRunState);
                    break;
                case RunState.PlayerTurn:
                    Game.EnableGameplaySystems();
                    MapView.Draw(currentRunState);
                    SidebarView.Draw(currentRunState);
                    RunStateStack.Pop();
                    break;
                case RunState.ShowTargeting:
                    Game.DisableGameplaySystems();
                    MapView.Draw(currentRunState);
                    TargetingView.Draw(currentRunState);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (Terminal.HasInput())
            {
                var key = Terminal.Read();
                switch (key)
                {
                    case Terminal.TK_RESIZED:
                        Resize();
                        break;
                    default:
                        var command = InputResolver.Resolve(RunStateStack.Peek(), key);
                        if (command != null) {
                            var (popCurrentState, pushState) = command.Act();
                            if (popCurrentState)
                            {
                                RunStateStack.Pop();
                            }

                            if (pushState != null)
                            {
                                RunStateStack.Push(pushState.Value);
                            }
                        }
                        break;
                }
            }
            
            Terminal.Refresh();
        }

        public void Resize()
        {
            var width = Terminal.State(Terminal.TK_WIDTH);
            var height = Terminal.State(Terminal.TK_HEIGHT);

            var cwidth = Terminal.State(Terminal.TK_CELL_WIDTH);
            var cheight = Terminal.State(Terminal.TK_CELL_HEIGHT);

            ScreenFrameAbs = new Rectangle(0, 0, width, height);
            MapView.ScreenFrameAbs = new Rectangle(ScreenFrameAbs.Left, ScreenFrameAbs.Top, ScreenFrameAbs.Right - 30 * SidebarView.SpacingX, ScreenFrameAbs.Bottom);
            SidebarView.ScreenFrameAbs = new Rectangle(MapView.ScreenFrameAbs.Right, ScreenFrameAbs.Top, ScreenFrameAbs.Width - MapView.ScreenFrameAbs.Width, ScreenFrameAbs.Bottom);
            TargetingView.ScreenFrameAbs = new Rectangle(MapView.ScreenFrameAbs.Right, ScreenFrameAbs.Top, ScreenFrameAbs.Width - MapView.ScreenFrameAbs.Width, ScreenFrameAbs.Bottom);
        }
    }
}
