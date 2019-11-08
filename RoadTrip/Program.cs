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
                god.Container.Resolve<ScriptLoader>();
                var rootView = god.Container.Resolve<RootView>();
                rootView.Tick();
                while (rootView.Game.Run) {
                    rootView.Tick();
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
