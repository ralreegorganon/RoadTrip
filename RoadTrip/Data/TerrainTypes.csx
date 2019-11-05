#r "RoadTrip"

TerrainType t(string name, char sym, Color fg)
{
    return new TerrainType {
        Name = name,
        Renderable = new Renderable {
            Symbol = sym,
            FgColor = fg
        }
    };
}

Codex.TerrainTypes = new List<TerrainType> {
    t("t_wall", '#', Color.WhiteSmoke),
    t("t_floor", '.', Color.WhiteSmoke)
};

