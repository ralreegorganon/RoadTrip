#r "RoadTrip"

Codex.TerrainTypes = new List<TerrainType> {
    new TerrainType("t_wall", "Wall", '#', ColorTranslator.FromHtml("#b1d1fc"), Color.Beige, true, false),
    new TerrainType("t_floor", "Floor", '.', ColorTranslator.FromHtml("#5e819d"), Color.Black, false, true)
};
