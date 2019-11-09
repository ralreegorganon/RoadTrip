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

            Terminal.Open();
            Terminal.Set("window: title='road trip', resizeable=true, size=80x24;");
            Terminal.Set("text font: ./Cascadia.ttf, size=16x16");
            Terminal.Set("font: ./Topaz-8.ttf, size=8, align=center");
            Terminal.Set("output: vsync=false;");

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
                    RunStateStack.Push(RunState.AwaitingInput);
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
            MapView.ScreenFrameAbs = new Rectangle(ScreenFrameAbs.Left + 1, ScreenFrameAbs.Top + 1, ScreenFrameAbs.Right - 20, ScreenFrameAbs.Bottom - 2);
            SidebarView.ScreenFrameAbs = new Rectangle(MapView.ScreenFrameAbs.Right + 2, ScreenFrameAbs.Top + 1, ScreenFrameAbs.Width - MapView.ScreenFrameAbs.Width - 4, ScreenFrameAbs.Bottom - 2);
            TargetingView.ScreenFrameAbs = new Rectangle(MapView.ScreenFrameAbs.Right + 2, ScreenFrameAbs.Top + 1, ScreenFrameAbs.Width - MapView.ScreenFrameAbs.Width - 4, ScreenFrameAbs.Bottom - 2);
        }
    }
}
