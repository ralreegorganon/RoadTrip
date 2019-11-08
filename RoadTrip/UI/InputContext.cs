using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using BearLib;
using RoadTrip.Game.Commands;

namespace RoadTrip.UI
{
    public enum InputContext
    {
        PlayerDirectControl,
        CursorDirectControl
    }

    public enum Command
    {
        Quit,
        MovePlayerNorth,
        MovePlayerSouth,
        MovePlayerEast,
        MovePlayerWest,
        MoveCursorNorth,
        MoveCursorSouth,
        MoveCursorEast,
        MoveCursorWest,
        TogglePlayerCursorCameraFocus,
        None
    }

    public class InputResolver
    {
        private Dictionary<(InputContext, int), Command?> KeyMapping { get; } 
        private Dictionary<Command, GameCommand> CommandMapping { get; } 

        public InputResolver()
        {
            KeyMapping = new Dictionary<(InputContext, int), Command?> {
                {(InputContext.PlayerDirectControl, Terminal.TK_UP), Command.MovePlayerNorth},
                {(InputContext.PlayerDirectControl, Terminal.TK_DOWN), Command.MovePlayerSouth},
                {(InputContext.PlayerDirectControl, Terminal.TK_LEFT), Command.MovePlayerWest},
                {(InputContext.PlayerDirectControl, Terminal.TK_RIGHT), Command.MovePlayerEast},
                {(InputContext.PlayerDirectControl, Terminal.TK_ENTER), Command.TogglePlayerCursorCameraFocus},
                {(InputContext.CursorDirectControl, Terminal.TK_UP), Command.MoveCursorNorth},
                {(InputContext.CursorDirectControl, Terminal.TK_DOWN), Command.MoveCursorSouth},
                {(InputContext.CursorDirectControl, Terminal.TK_LEFT), Command.MoveCursorWest},
                {(InputContext.CursorDirectControl, Terminal.TK_RIGHT), Command.MoveCursorEast},
                {(InputContext.CursorDirectControl, Terminal.TK_CLOSE), Command.Quit},
                {(InputContext.CursorDirectControl, Terminal.TK_ENTER), Command.TogglePlayerCursorCameraFocus},
            };

            CommandMapping = new Dictionary<Command, GameCommand> {
                {Command.None, new Noop()},
                {Command.Quit, new Quit()},
                {Command.MovePlayerNorth, new MovePlayerNorth()},
                {Command.MovePlayerSouth, new MovePlayerSouth()},
                {Command.MovePlayerEast, new MovePlayerEast()},
                {Command.MovePlayerWest, new MovePlayerWest()},
                {Command.MoveCursorNorth, new MoveCursorNorth()},
                {Command.MoveCursorSouth, new MoveCursorSouth()},
                {Command.MoveCursorEast, new MoveCursorEast()},
                {Command.MoveCursorWest, new MoveCursorWest()},
                {Command.TogglePlayerCursorCameraFocus, new TogglePlayerCursorCameraFocus()},
            };
        }

        public GameCommand Resolve(InputContext context, int key)
        {
            KeyMapping.TryGetValue((context, key), out var keyCommand);
            if (keyCommand == null) {
                return new Noop();
            }

            CommandMapping.TryGetValue(keyCommand.Value, out var gameCommand);
            return gameCommand ?? new Noop();
        }
    }
}
