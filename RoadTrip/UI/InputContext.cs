using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using BearLib;
using RoadTrip.Game.Commands;

namespace RoadTrip.UI
{
    

    public class InputResolver
    {
        private Dictionary<(RunState, int), Command?> KeyMapping { get; } 
        private Dictionary<Command, GameCommand> CommandMapping { get; } 

        public InputResolver(GameCommand[] commands)
        {
            KeyMapping = new Dictionary<(RunState, int), Command?> {
                {(RunState.AwaitingInput, Terminal.TK_UP), Command.MovePlayerNorth},
                {(RunState.AwaitingInput, Terminal.TK_DOWN), Command.MovePlayerSouth},
                {(RunState.AwaitingInput, Terminal.TK_LEFT), Command.MovePlayerWest},
                {(RunState.AwaitingInput, Terminal.TK_RIGHT), Command.MovePlayerEast},
                {(RunState.AwaitingInput, Terminal.TK_KP_8), Command.MovePlayerNorth},
                {(RunState.AwaitingInput, Terminal.TK_KP_2), Command.MovePlayerSouth},
                {(RunState.AwaitingInput, Terminal.TK_KP_4), Command.MovePlayerWest},
                {(RunState.AwaitingInput, Terminal.TK_KP_6), Command.MovePlayerEast},
                {(RunState.AwaitingInput, Terminal.TK_KP_7), Command.MovePlayerNorthWest},
                {(RunState.AwaitingInput, Terminal.TK_KP_9), Command.MovePlayerNorthEast},
                {(RunState.AwaitingInput, Terminal.TK_KP_1), Command.MovePlayerSouthWest},
                {(RunState.AwaitingInput, Terminal.TK_KP_3), Command.MovePlayerSouthEast},
                {(RunState.AwaitingInput, Terminal.TK_F), Command.ShowTargeting},

                {(RunState.ShowTargeting, Terminal.TK_UP), Command.MoveCursorNorth},
                {(RunState.ShowTargeting, Terminal.TK_DOWN), Command.MoveCursorSouth},
                {(RunState.ShowTargeting, Terminal.TK_LEFT), Command.MoveCursorWest},
                {(RunState.ShowTargeting, Terminal.TK_RIGHT), Command.MoveCursorEast},
                {(RunState.ShowTargeting, Terminal.TK_KP_8), Command.MoveCursorNorth},
                {(RunState.ShowTargeting, Terminal.TK_KP_2), Command.MoveCursorSouth},
                {(RunState.ShowTargeting, Terminal.TK_KP_4), Command.MoveCursorWest},
                {(RunState.ShowTargeting, Terminal.TK_KP_6), Command.MoveCursorEast},
                {(RunState.ShowTargeting, Terminal.TK_KP_7), Command.MoveCursorNorthWest},
                {(RunState.ShowTargeting, Terminal.TK_KP_9), Command.MoveCursorNorthEast},
                {(RunState.ShowTargeting, Terminal.TK_KP_1), Command.MoveCursorSouthWest},
                {(RunState.ShowTargeting, Terminal.TK_KP_3), Command.MoveCursorSouthEast},
                {(RunState.ShowTargeting, Terminal.TK_CLOSE), Command.Quit},
                {(RunState.ShowTargeting, Terminal.TK_ENTER), Command.CommitTargeting},
                {(RunState.ShowTargeting, Terminal.TK_ESCAPE), Command.CancelTargeting},
            };

            CommandMapping = commands.ToDictionary(k => k.Command, v => v);
        }

        public GameCommand? Resolve(RunState context, int key)
        {
            KeyMapping.TryGetValue((context, key), out var keyCommand);
            if (keyCommand == null) {
                return null;
            }

            CommandMapping.TryGetValue(keyCommand.Value, out var gameCommand);
            return gameCommand;
        }
    }
}
