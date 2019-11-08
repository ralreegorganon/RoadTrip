using System;
using BearLib;
using DryIoc;
using RoadTrip.Game;
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

                var inputResolver = god.Container.Resolve<InputResolver>();

                god.Container.Resolve<ScriptLoader>();

                var game = god.Container.Resolve<Game.Game>();
                var rootView = god.Container.Resolve<RootView>();

                game.Setup();
                game.Tick();
                rootView.Draw();

                while (game.Run) {
                    if (Terminal.HasInput()) {
                        var key = Terminal.Read();

                        switch (key)
                        {
                            case Terminal.TK_RESIZED:
                                rootView.Resize();
                                break;
                            default:
                                var command = inputResolver.Resolve(rootView.CurrentInputContext, key);
                                command.Act(game, rootView);
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
