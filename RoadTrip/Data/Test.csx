#r "RoadTrip"

using System;
using System.Collections.Generic;
using System.Drawing;
using RoadTrip.Game;
using RoadTrip.Game.Components;

Terrain t(char sym, Color c)
{
    return new Terrain {
        Renderable = new Renderable {
            Symbol = sym,
            Color = c
        }
    };
}

GlobalGame.Terrains = new List<Terrain> {
    t('C', Color.Red),
    t('Z', Color.Red)
};

var r = GlobalGame.Player.Get<Renderable>();
r.Symbol = 'Z';
r.Color = Color.Blue;
