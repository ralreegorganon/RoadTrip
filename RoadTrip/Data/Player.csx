#r "RoadTrip"

Game.Player = World.NewEntityWith<Position, Renderable, PlayerControllable, CameraFocusTag>(out Position playerPosition, out Renderable playerRenderable, out _, out _);
playerPosition.Coordinate = new Coordinate(0, 0, 0);
playerRenderable.Color = Color.Red;
playerRenderable.Symbol = '@';