using System.Collections.Generic;
using Leopotam.Ecs;

namespace RoadTrip.Game
{
    public class Map
    {
        public Dictionary<Coordinate, TerrainType> Terrain { get; set; } = new Dictionary<Coordinate, TerrainType>();

        public Dictionary<Coordinate, List<EcsEntity>> TileEntities { get; set; } = new Dictionary<Coordinate, List<EcsEntity>>();
    }
}
