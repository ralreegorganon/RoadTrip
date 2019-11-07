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
    t("t_wall", '#', ColorTranslator.FromHtml("#b1d1fc"), true),
    t("t_floor", '.', ColorTranslator.FromHtml("#5e819d"), false)
};
