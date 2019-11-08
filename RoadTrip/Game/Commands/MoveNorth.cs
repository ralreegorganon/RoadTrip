using RoadTrip.Game.Components;
using RoadTrip.UI;

namespace RoadTrip.Game.Commands
{
    public abstract class GameCommand
    {
        public abstract void Act(Game game, RootView view);
    }

    public class Noop : GameCommand
    {
        public override void Act(Game game, RootView view)
        {
        }
    }

    public class Quit : GameCommand
    {
        public override void Act(Game game, RootView view)
        {
            game.Run = false;
        }
    }

    public class MovePlayerNorth : GameCommand
    {
        public override void Act(Game game, RootView view)
        {
            var wtm = game.Player.Set<WantsToMove>();
            wtm.Movement = new Coordinate(0, -1, 0);
        }
    }

    public class MovePlayerSouth : GameCommand
    {
        public override void Act(Game game, RootView view)
        {
            var wtm = game.Player.Set<WantsToMove>();
            wtm.Movement = new Coordinate(0, 1, 0);
        }
    }

    public class MovePlayerEast : GameCommand
    {
        public override void Act(Game game, RootView view)
        {
            var wtm = game.Player.Set<WantsToMove>();
            wtm.Movement = new Coordinate(1, 0, 0);
        }
    }

    public class MovePlayerWest : GameCommand
    {
        public override void Act(Game game, RootView view)
        {
            var wtm = game.Player.Set<WantsToMove>();
            wtm.Movement = new Coordinate(-1, 0, 0);
        }
    }

    public class MoveCursorNorth : GameCommand
    {
        public override void Act(Game game, RootView view)
        {
            var wtm = game.Cursor.Set<WantsToMove>();
            wtm.Movement = new Coordinate(0, -1, 0);
        }
    }

    public class MoveCursorSouth : GameCommand
    {
        public override void Act(Game game, RootView view)
        {
            var wtm = game.Cursor.Set<WantsToMove>();
            wtm.Movement = new Coordinate(0, 1, 0);
        }
    }

    public class MoveCursorEast : GameCommand
    {
        public override void Act(Game game, RootView view)
        {
            var wtm = game.Cursor.Set<WantsToMove>();
            wtm.Movement = new Coordinate(1, 0, 0);
        }
    }

    public class MoveCursorWest : GameCommand
    {
        public override void Act(Game game, RootView view)
        {
            var wtm = game.Cursor.Set<WantsToMove>();
            wtm.Movement = new Coordinate(-1, 0, 0);
        }
    }

    public class TogglePlayerCursorCameraFocus : GameCommand
    {
        public override void Act(Game game, RootView view)
        {
            if (game.Player.Get<CameraFocusTag>() != null)
            {
                game.Player.Unset<CameraFocusTag>();
                game.Cursor.Set<CameraFocusTag>();
                view.PushInputContext(InputContext.CursorDirectControl);
            }
            else
            {
                game.Player.Set<CameraFocusTag>();
                game.Cursor.Unset<CameraFocusTag>();
                view.PopInputContext();
            }
        }
    }
}
