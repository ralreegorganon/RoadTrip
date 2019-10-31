#r "RoadTrip"

Terrain t(char sym, Color c)
{
    return new Terrain {
        Renderable = new Renderable {
            Symbol = sym,
            Color = c
        }
    };
}

Codex.Terrains = new List<Terrain> {
    t('C', Color.Red),
    t('Z', Color.Red)
};

