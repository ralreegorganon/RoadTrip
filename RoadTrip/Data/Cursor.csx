#r "RoadTrip"

Game.Cursor = World.NewEntityWith<Position, CursorTag>(out Position cursorPosition, out _);
cursorPosition.Coordinate = new Coordinate(0, 0, 0);
