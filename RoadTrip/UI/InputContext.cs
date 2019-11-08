using System.Collections.Generic;
using System.Runtime.InteropServices;
using BearLib;
using RoadTrip.Game.Commands;

namespace RoadTrip.UI
{
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
        ShowTargeting,
        CancelTargeting,
        CommitTargeting,
        TogglePlayerCursorCameraFocus,
        None
    }

    public class InputResolver
    {
        private Dictionary<(RunState, int), Command?> KeyMapping { get; } 
        private Dictionary<Command, GameCommand> CommandMapping { get; } 

        public InputResolver()
        {
            KeyMapping = new Dictionary<(RunState, int), Command?> {
                {(RunState.AwaitingInput, Terminal.TK_UP), Command.MovePlayerNorth},
                {(RunState.AwaitingInput, Terminal.TK_DOWN), Command.MovePlayerSouth},
                {(RunState.AwaitingInput, Terminal.TK_LEFT), Command.MovePlayerWest},
                {(RunState.AwaitingInput, Terminal.TK_RIGHT), Command.MovePlayerEast},
                {(RunState.AwaitingInput, Terminal.TK_F), Command.ShowTargeting},
                {(RunState.ShowTargeting, Terminal.TK_UP), Command.MoveCursorNorth},
                {(RunState.ShowTargeting, Terminal.TK_DOWN), Command.MoveCursorSouth},
                {(RunState.ShowTargeting, Terminal.TK_LEFT), Command.MoveCursorWest},
                {(RunState.ShowTargeting, Terminal.TK_RIGHT), Command.MoveCursorEast},
                {(RunState.ShowTargeting, Terminal.TK_CLOSE), Command.Quit},
                {(RunState.ShowTargeting, Terminal.TK_ENTER), Command.CommitTargeting},
                {(RunState.ShowTargeting, Terminal.TK_ESCAPE), Command.CancelTargeting},
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
                {Command.ShowTargeting, new ShowTargeting()},
                {Command.CancelTargeting, new CancelTargeting()},
                {Command.CommitTargeting, new CommitTargeting()},
            };
        }

        public GameCommand Resolve(RunState context, int key)
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
