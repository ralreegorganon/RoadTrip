#r "RoadTrip"

Game.Player = World.NewEntityWith<Position, Renderable, PlayerControllable, CameraFocusTag>(out Position playerPosition, out Renderable playerRenderable, out _, out _);

playerPosition.Coordinate = new Coordinate(0, 0, 0);
playerRenderable.FgColor = Color.Red;
playerRenderable.Symbol = '@';

var viewshed = Game.Player.Set<Viewshed>();
viewshed.Visible = new HashSet<Coordinate>();
viewshed.Range = 12;
viewshed.Dirty = true;