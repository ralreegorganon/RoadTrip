#r "RoadTrip"

Game.Cursor = World.NewEntityWith<Position, Renderable, CursorTag>(out Position cursorPosition, out Renderable cursorRenderable, out _);
cursorPosition.Coordinate = new Coordinate(0, 0, 0);
cursorRenderable.FgColor = Color.Green;
cursorRenderable.Symbol = 'x';