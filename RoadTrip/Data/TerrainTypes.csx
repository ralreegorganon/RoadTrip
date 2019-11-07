#r "RoadTrip"

TerrainType t(string name, char sym, Color fg, bool isOpaque)
{
    return new TerrainType {
        Name = name,
        Renderable = new Renderable {
            Symbol = sym,
            FgColor = fg
        },
        IsOpaque = isOpaque
    };
}

Codex.TerrainTypes = new List<TerrainType> {
    t("t_wall", '#', Color.WhiteSmoke, true),
    t("t_floor", '.', Color.WhiteSmoke, false)
};

