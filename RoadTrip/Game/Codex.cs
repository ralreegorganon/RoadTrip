using System.Collections.Generic;
using System.Drawing;
using Leopotam.Ecs;
using RoadTrip.Game.Blueprints;
using RoadTrip.Game.Components;

namespace RoadTrip.Game
{
    public class Codex
    {
        public List<TerrainType> TerrainTypes { get; set; } = new List<TerrainType>();

        public Dictionary<string, TerrainType> TerrainLookup { get; set; } = new Dictionary<string, TerrainType>();

        public List<MobBlueprint> Mobs { get; set; } = new List<MobBlueprint>();

        public Dictionary<string, MobBlueprint> MobsLookup { get; set; } = new Dictionary<string, MobBlueprint>();

        public EcsEntity SpawnMobAtPosition(EcsWorld world, string id, Coordinate spawnAt)
        {
            var mob = MobsLookup[id];

            var entity = world.NewEntity();

            var nameable = entity.Set<Nameable>();
            nameable.Name = mob.Name;

            var renderable = entity.Set<Renderable>();
            renderable.Symbol = mob.Renderable.Symbol;
            renderable.BgColor = Color.Black;
            renderable.FgColor = mob.Renderable.Color;

            var position = entity.Set<Position>();
            position.Coordinate = spawnAt;

            return entity;
        }
    }
}
