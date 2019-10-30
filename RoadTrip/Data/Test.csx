#r "RoadTrip"

using System.Drawing;
using RoadTrip.Game;
using RoadTrip.Game.Components;

var t = new Terrain
{
    Renderable = new Renderable
    {
        Symbol = '#',
        Color = System.Drawing.Color.Red
    }
};

GlobalGame.Terrains.Add(t);