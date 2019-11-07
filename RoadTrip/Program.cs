using System;
using BearLib;
using DryIoc;
using Leopotam.Ecs;
using RoadTrip.Game;
using RoadTrip.Game.Components;
using RoadTrip.UI;
using Serilog;

namespace RoadTrip
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Debug(outputTemplate: "[{Timestamp:yyyyMMdd HH:mm:ss} {Level} {SourceContext}] - {Message:lj}{NewLine}{Exception}")
                .WriteTo.File("log.txt", outputTemplate: "[{Timestamp:yyyyMMdd HH:mm:ss} {Level} {SourceContext}] - {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            try {
                Log.Information("Starting up");

                var god = new God();

                god.Container.Resolve<ScriptLoader>();

                var game = god.Container.Resolve<Game.Game>();
                var rootView = god.Container.Resolve<RootView>();

                game.Setup();
                game.Tick();
                rootView.Draw();

                while (game.Run) {
                    if (Terminal.HasInput()) {
                        var key = Terminal.Read();
                        EcsEntity? t = null;
                        if (game.Player.Get<CameraFocusTag>() != null) {
                            t = game.Player;
                        }
                        else if (game.Cursor.Get<CameraFocusTag>() != null) {
                            t = game.Cursor;
                        }

                        switch (key) {
                            case Terminal.TK_LEFT:
                                if (t != null) {
                                    var wtm = t.Value.Set<WantsToMove>();
                                    wtm.Movement = new Coordinate(-1, 0, 0);
                                }

                                break;
                            case Terminal.TK_RIGHT:
                                if (t != null) {
                                    var wtm = t.Value.Set<WantsToMove>();
                                    wtm.Movement = new Coordinate(1, 0, 0);
                                }

                                break;
                            case Terminal.TK_UP:
                                if (t != null) {
                                    var wtm = t.Value.Set<WantsToMove>();
                                    wtm.Movement = new Coordinate(0, -1, 0);
                                }

                                break;
                            case Terminal.TK_DOWN:
                                if (t != null) {
                                    var wtm = t.Value.Set<WantsToMove>();
                                    wtm.Movement = new Coordinate(0, 1, 0);
                                }

                                break;
                            case Terminal.TK_ENTER:
                                if (game.Player.Get<CameraFocusTag>() != null) {
                                    game.Player.Unset<CameraFocusTag>();
                                    game.Cursor.Set<CameraFocusTag>();
                                }
                                else {
                                    game.Player.Set<CameraFocusTag>();
                                    game.Cursor.Unset<CameraFocusTag>();
                                }

                                break;
                            case Terminal.TK_CLOSE:
                                game.Run = false;
                                break;
                            case Terminal.TK_RESIZED:
                                rootView.Resize();
                                break;
                        }
                    }

                    game.Tick();
                    rootView.Draw();
                }

                Terminal.Close();

                Log.Information("Exiting");
            }
            catch (Exception e) {
                Log.Fatal(e, "Unhandled exception");
                throw;
            }
        }
    }
}
