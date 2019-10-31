using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Leopotam.Ecs;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Serilog;

namespace RoadTrip.Game
{
    public class ScriptGlobals
    {
        public Game Game;
        public EcsWorld World;
        public Codex Codex;
    }

    public class Codex
    {
        private ILogger Logger { get; }

        public List<Terrain> Terrains { get; set; } = new List<Terrain>();

        public Codex(EcsWorld world, Game game, ILogger logger)
        {
            Logger = logger;

            var scriptGlobals = new ScriptGlobals {Codex = this, World = world, Game = game};

            var opts = ScriptOptions.Default.AddImports(new [] {
                "System",
                "System.Collections.Generic",
                "System.Drawing",
                "RoadTrip.Game",
                "RoadTrip.Game.Components"
            });

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.IsDynamic == false);
            foreach (var assembly in assemblies)
            {
                opts.AddReferences(assembly);
            }

            var allScripts = Directory.GetFiles(Path.Join(AppContext.BaseDirectory, "Data"), "*.csx", SearchOption.AllDirectories)
                .OrderBy(x => x)
                .ToList();

            foreach (var scriptPath in allScripts) {
                var code = File.ReadAllText(scriptPath);
                var script = CSharpScript.Create(code, opts, typeof(ScriptGlobals));
                var compilation = script.GetCompilation();
                var diagnostics = compilation.GetDiagnostics();
                if (diagnostics.Any()) {
                    var fail = false;
                    foreach (var diagnostic in diagnostics)
                    {
                        if (diagnostic.WarningLevel == 0) {
                            fail = true;
                            Logger.Fatal("Failed to execute {Script}: {Message}", scriptPath, diagnostic.ToString());
                        }
                        else {
                            Logger.Error("Failed to execute {Script}: {Message}", scriptPath, diagnostic.ToString());
                        }
                    }
                    if (fail) {
                        throw new Exception("Aborted due to script errors");
                    }
                }
                
                var result = script.RunAsync(globals: scriptGlobals).Result;
            }
        }
    }
}
