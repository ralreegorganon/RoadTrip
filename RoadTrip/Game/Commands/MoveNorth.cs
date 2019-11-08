using RoadTrip.Game.Components;
using RoadTrip.UI;

namespace RoadTrip.Game.Commands
{
    public abstract class GameCommand
    {
        public abstract (bool PopCurrentState, RunState? PushState) Act(Game game);
    }

    public class Noop : GameCommand
    {
        public override (bool PopCurrentState, RunState? PushState) Act(Game game)
        {
            return (false, null);
        }
    }

    public class ShowTargeting : GameCommand
    {
        public override (bool PopCurrentState, RunState? PushState) Act(Game game)
        {
            game.Player.Unset<CameraFocusTag>();
            game.Cursor.Set<CameraFocusTag>();

            return (false, RunState.ShowTargeting);
        }
    }

    public class CancelTargeting : GameCommand
    {
        public override (bool PopCurrentState, RunState? PushState) Act(Game game)
        {
            game.Player.Set<CameraFocusTag>();
            game.Cursor.Unset<CameraFocusTag>();

            return (true, null);
        }
    }

    public class CommitTargeting : GameCommand
    {
        public override (bool PopCurrentState, RunState? PushState) Act(Game game)
        {
            game.Player.Set<CameraFocusTag>();
            game.Cursor.Unset<CameraFocusTag>();

            return (true, null);
        }
    }


    public class Quit : GameCommand
    {
        public override (bool PopCurrentState, RunState? PushState) Act(Game game)
        {
            game.Run = false;

            return (true, null);
        }
    }

    public class MovePlayerNorth : GameCommand
    {
        public override (bool PopCurrentState, RunState? PushState) Act(Game game)
        {
            var wtm = game.Player.Set<WantsToMove>();
            wtm.Movement = new Coordinate(0, -1, 0);

            return (false, RunState.PlayerTurn);
        }
    }

    public class MovePlayerSouth : GameCommand
    {
        public override (bool PopCurrentState, RunState? PushState) Act(Game game)
        {
            var wtm = game.Player.Set<WantsToMove>();
            wtm.Movement = new Coordinate(0, 1, 0);

            return (false, RunState.PlayerTurn);
        }
    }

    public class MovePlayerEast : GameCommand
    {
        public override (bool PopCurrentState, RunState? PushState) Act(Game game)
        {
            var wtm = game.Player.Set<WantsToMove>();
            wtm.Movement = new Coordinate(1, 0, 0);

            return (false, RunState.PlayerTurn);
        }
    }

    public class MovePlayerWest : GameCommand
    {
        public override (bool PopCurrentState, RunState? PushState) Act(Game game)
        {
            var wtm = game.Player.Set<WantsToMove>();
            wtm.Movement = new Coordinate(-1, 0, 0);

            return (false, RunState.PlayerTurn);
        }
    }

    public class MoveCursorNorth : GameCommand
    {
        public override (bool PopCurrentState, RunState? PushState) Act(Game game)
        {
            var wtm = game.Cursor.Set<WantsToMove>();
            wtm.Movement = new Coordinate(0, -1, 0);

            return (false, null);
        }
    }

    public class MoveCursorSouth : GameCommand
    {
        public override (bool PopCurrentState, RunState? PushState) Act(Game game)
        {
            var wtm = game.Cursor.Set<WantsToMove>();
            wtm.Movement = new Coordinate(0, 1, 0);

            return (false, null);
        }
    }

    public class MoveCursorEast : GameCommand
    {
        public override (bool PopCurrentState, RunState? PushState) Act(Game game)
        {
            var wtm = game.Cursor.Set<WantsToMove>();
            wtm.Movement = new Coordinate(1, 0, 0);

            return (false, null);
        }
    }

    public class MoveCursorWest : GameCommand
    {
        public override (bool PopCurrentState, RunState? PushState) Act(Game game)
        {
            var wtm = game.Cursor.Set<WantsToMove>();
            wtm.Movement = new Coordinate(-1, 0, 0);

            return (false, null);
        }
    }

    public class TogglePlayerCursorCameraFocus : GameCommand
    {
        public override (bool PopCurrentState, RunState? PushState) Act(Game game)
        {
            //if (game.Player.Get<CameraFocusTag>() != null)
            //{
            //    game.Player.Unset<CameraFocusTag>();
            //    game.Cursor.Set<CameraFocusTag>();
            //    view.PushInputContext(InputContext.CursorDirectControl);
            //}
            //else
            //{
            //    game.Player.Set<CameraFocusTag>();
            //    game.Cursor.Unset<CameraFocusTag>();
            //    view.PopInputContext();
            //}

            return (false, null);
        }
    }
}
