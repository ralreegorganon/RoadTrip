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
    public class Codex
    {
        public List<TerrainType> TerrainTypes { get; set; } = new List<TerrainType>();

        public Dictionary<string, TerrainType> TerrainLookup { get; set; } = new Dictionary<string, TerrainType>();
    }
}
