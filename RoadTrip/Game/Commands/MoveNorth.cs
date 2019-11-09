using Leopotam.Ecs;
using RoadTrip.Game.Components;
using RoadTrip.UI;

namespace RoadTrip.Game.Commands
{
    public enum Command
    {
        Quit,
        MovePlayerNorth,
        MovePlayerSouth,
        MovePlayerEast,
        MovePlayerWest,
        MovePlayerNorthWest,
        MovePlayerNorthEast,
        MovePlayerSouthWest,
        MovePlayerSouthEast,
        MoveCursorNorth,
        MoveCursorSouth,
        MoveCursorEast,
        MoveCursorWest,
        MoveCursorNorthWest,
        MoveCursorNorthEast,
        MoveCursorSouthWest,
        MoveCursorSouthEast,
        ShowTargeting,
        CancelTargeting,
        CommitTargeting,
        TogglePlayerCursorCameraFocus,
        None
    }

    public abstract class GameCommand
    {
        protected Game Game { get; }
        protected EcsWorld World { get; }

        protected GameCommand(Game game, EcsWorld world)
        {
            Game = game;
            World = world;
        }

        public Command Command { get; set; }

        public abstract (bool PopCurrentState, RunState? PushState) Act();
    }

    public class Noop : GameCommand
    {
        public Noop(Game game, EcsWorld world) : base(game, world)
        {
            Command = Command.None;
        }

        public override (bool PopCurrentState, RunState? PushState) Act()
        {
            return (false, null);
        }
    }

    public class ShowTargeting : GameCommand
    {
        public ShowTargeting(Game game, EcsWorld world) : base(game, world)
        {
            Command = Command.ShowTargeting;
        }

        public override (bool PopCurrentState, RunState? PushState) Act()
        {
            //Game.Player.Unset<CameraFocusTag>();
            //Game.Cursor.Set<CameraFocusTag>();

            var cameraFocusFilter = World.GetFilter(typeof(EcsFilter<Position, CameraFocusTag>));
            ref var cameraFocusEntity = ref cameraFocusFilter.Entities[0];
            var cameraPos = cameraFocusEntity.Get<Position>();
            var cursorPos = Game.Cursor.Get<Position>();
            cursorPos.Coordinate = cameraPos.Coordinate;

            return (false, RunState.ShowTargeting);
        }
    }

    public class CancelTargeting : GameCommand
    {
        public CancelTargeting(Game game, EcsWorld world) : base(game, world)
        {
            Command = Command.CancelTargeting;
        }

        public override (bool PopCurrentState, RunState? PushState) Act()
        {
            //Game.Player.Set<CameraFocusTag>();
            //Game.Cursor.Unset<CameraFocusTag>();

            return (true, null);
        }
    }

    public class CommitTargeting : GameCommand
    {
        public CommitTargeting(Game game, EcsWorld world) : base(game, world)
        {
            Command = Command.CommitTargeting;
        }

        public override (bool PopCurrentState, RunState? PushState) Act()
        {
            Game.Player.Set<CameraFocusTag>();
            Game.Cursor.Unset<CameraFocusTag>();

            return (true, null);
        }
    }


    public class Quit : GameCommand
    {
        public Quit(Game game, EcsWorld world) : base(game, world)
        {
            Command = Command.Quit;
        }

        public override (bool PopCurrentState, RunState? PushState) Act()
        {
            Game.Run = false;

            return (true, null);
        }
    }

    public class MovePlayerNorth : GameCommand
    {
        public MovePlayerNorth(Game game, EcsWorld world) : base(game, world)
        {
            Command = Command.MovePlayerNorth;
        }

        public override (bool PopCurrentState, RunState? PushState) Act()
        {
            var wtm = Game.Player.Set<WantsToMove>();
            wtm.Movement = new Coordinate(0, -1, 0);

            return (false, RunState.PlayerTurn);
        }
    }

    public class MovePlayerSouth : GameCommand
    {
        public MovePlayerSouth(Game game, EcsWorld world) : base(game, world)
        {
            Command = Command.MovePlayerSouth;
        }

        public override (bool PopCurrentState, RunState? PushState) Act()
        {
            var wtm = Game.Player.Set<WantsToMove>();
            wtm.Movement = new Coordinate(0, 1, 0);

            return (false, RunState.PlayerTurn);
        }
    }

    public class MovePlayerEast : GameCommand
    {
        public MovePlayerEast(Game game, EcsWorld world) : base(game, world)
        {
            Command = Command.MovePlayerEast;
        }

        public override (bool PopCurrentState, RunState? PushState) Act()
        {
            var wtm = Game.Player.Set<WantsToMove>();
            wtm.Movement = new Coordinate(1, 0, 0);

            return (false, RunState.PlayerTurn);
        }
    }

    public class MovePlayerWest : GameCommand
    {
        public MovePlayerWest(Game game, EcsWorld world) : base(game, world)
        {
            Command = Command.MovePlayerWest;
        }

        public override (bool PopCurrentState, RunState? PushState) Act()
        {
            var wtm = Game.Player.Set<WantsToMove>();
            wtm.Movement = new Coordinate(-1, 0, 0);

            return (false, RunState.PlayerTurn);
        }
    }

    public class MovePlayerNorthWest : GameCommand
    {
        public MovePlayerNorthWest(Game game, EcsWorld world) : base(game, world)
        {
            Command = Command.MovePlayerNorthWest;
        }

        public override (bool PopCurrentState, RunState? PushState) Act()
        {
            var wtm = Game.Player.Set<WantsToMove>();
            wtm.Movement = new Coordinate(-1, -1, 0);

            return (false, RunState.PlayerTurn);
        }
    }

    public class MovePlayerSouthWest : GameCommand
    {
        public MovePlayerSouthWest(Game game, EcsWorld world) : base(game, world)
        {
            Command = Command.MovePlayerSouthWest;
        }

        public override (bool PopCurrentState, RunState? PushState) Act()
        {
            var wtm = Game.Player.Set<WantsToMove>();
            wtm.Movement = new Coordinate(-1, 1, 0);

            return (false, RunState.PlayerTurn);
        }
    }

    public class MovePlayerNorthEast : GameCommand
    {
        public MovePlayerNorthEast(Game game, EcsWorld world) : base(game, world)
        {
            Command = Command.MovePlayerNorthEast;
        }

        public override (bool PopCurrentState, RunState? PushState) Act()
        {
            var wtm = Game.Player.Set<WantsToMove>();
            wtm.Movement = new Coordinate(1, -1, 0);

            return (false, RunState.PlayerTurn);
        }
    }

    public class MovePlayerSouthEast : GameCommand
    {
        public MovePlayerSouthEast(Game game, EcsWorld world) : base(game, world)
        {
            Command = Command.MovePlayerSouthEast;
        }

        public override (bool PopCurrentState, RunState? PushState) Act()
        {
            var wtm = Game.Player.Set<WantsToMove>();
            wtm.Movement = new Coordinate(1, 1, 0);

            return (false, RunState.PlayerTurn);
        }
    }

    public class MoveCursorNorth : GameCommand
    {
        public MoveCursorNorth(Game game, EcsWorld world) : base(game, world)
        {
            Command = Command.MoveCursorNorth;
        }

        public override (bool PopCurrentState, RunState? PushState) Act()
        {
            var wtm = Game.Cursor.Set<WantsToMove>();
            wtm.Movement = new Coordinate(0, -1, 0);

            return (false, null);
        }
    }

    public class MoveCursorSouth : GameCommand
    {
        public MoveCursorSouth(Game game, EcsWorld world) : base(game, world)
        {
            Command = Command.MoveCursorSouth;
        }

        public override (bool PopCurrentState, RunState? PushState) Act()
        {
            var wtm = Game.Cursor.Set<WantsToMove>();
            wtm.Movement = new Coordinate(0, 1, 0);

            return (false, null);
        }
    }

    public class MoveCursorEast : GameCommand
    {
        public MoveCursorEast(Game game, EcsWorld world) : base(game, world)
        {
            Command = Command.MoveCursorEast;
        }

        public override (bool PopCurrentState, RunState? PushState) Act()
        {
            var wtm = Game.Cursor.Set<WantsToMove>();
            wtm.Movement = new Coordinate(1, 0, 0);

            return (false, null);
        }
    }

    public class MoveCursorWest : GameCommand
    {
        public MoveCursorWest(Game game, EcsWorld world) : base(game, world)
        {
            Command = Command.MoveCursorWest;
        }

        public override (bool PopCurrentState, RunState? PushState) Act()
        {
            var wtm = Game.Cursor.Set<WantsToMove>();
            wtm.Movement = new Coordinate(-1, 0, 0);

            return (false, null);
        }
    }

    public class MoveCursorNorthWest : GameCommand
    {
        public MoveCursorNorthWest(Game game, EcsWorld world) : base(game, world)
        {
            Command = Command.MoveCursorNorthWest;
        }

        public override (bool PopCurrentState, RunState? PushState) Act()
        {
            var wtm = Game.Cursor.Set<WantsToMove>();
            wtm.Movement = new Coordinate(-1, -1, 0);

            return (false, null);
        }
    }

    public class MoveCursorSouthWest : GameCommand
    {
        public MoveCursorSouthWest(Game game, EcsWorld world) : base(game, world)
        {
            Command = Command.MoveCursorSouthWest;
        }

        public override (bool PopCurrentState, RunState? PushState) Act()
        {
            var wtm = Game.Cursor.Set<WantsToMove>();
            wtm.Movement = new Coordinate(-1, 1, 0);

            return (false, null);
        }
    }

    public class MoveCursorNorthEast : GameCommand
    {
        public MoveCursorNorthEast(Game game, EcsWorld world) : base(game, world)
        {
            Command = Command.MoveCursorNorthEast;
        }

        public override (bool PopCurrentState, RunState? PushState) Act()
        {
            var wtm = Game.Cursor.Set<WantsToMove>();
            wtm.Movement = new Coordinate(1, -1, 0);

            return (false, null);
        }
    }

    public class MoveCursorSouthEast : GameCommand
    {
        public MoveCursorSouthEast(Game game, EcsWorld world) : base(game, world)
        {
            Command = Command.MoveCursorSouthEast;
        }

        public override (bool PopCurrentState, RunState? PushState) Act()
        {
            var wtm = Game.Cursor.Set<WantsToMove>();
            wtm.Movement = new Coordinate(1, 1, 0);

            return (false, null);
        }
    }

    public class TogglePlayerCursorCameraFocus : GameCommand
    {
        public TogglePlayerCursorCameraFocus(Game game, EcsWorld world) : base(game, world)
        {
            Command = Command.TogglePlayerCursorCameraFocus;
        }

        public override (bool PopCurrentState, RunState? PushState) Act()
        {
            //if (Game.Player.Get<CameraFocusTag>() != null)
            //{
            //    Game.Player.Unset<CameraFocusTag>();
            //    Game.Cursor.Set<CameraFocusTag>();
            //    view.PushInputContext(InputContext.CursorDirectControl);
            //}
            //else
            //{
            //    Game.Player.Set<CameraFocusTag>();
            //    Game.Cursor.Unset<CameraFocusTag>();
            //    view.PopInputContext();
            //}

            return (false, null);
        }
    }
}
