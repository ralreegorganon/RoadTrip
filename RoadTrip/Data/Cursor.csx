#r "RoadTrip"

Game.Cursor = World.NewEntityWith<Position, CursorTag, IncorporealTag>(out Position cursorPosition, out _, out _);
cursorPosition.Coordinate = new Coordinate(0, 0, 0);
