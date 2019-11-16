using System.Drawing;
using BearLib;

namespace RoadTrip.UI
{
    public abstract class View
    {
        public Rectangle ScreenFrameAbs { get; set; }

        public string Font { get; set; } = string.Empty;

        public int SpacingX { get; set; }

        public int SpacingY { get; set; }

        protected RunState CurrentRunState { get; set; }

        public virtual void Draw(RunState currentRunState)
        {
            CurrentRunState = currentRunState;
            Terminal.Font("");
            Terminal.Layer(0);
            Terminal.BkColor(Color.Black);
            Terminal.Color(Color.White);
        }

        public void Put(int x, int y, char code)
        {
            if (x >= 0 && x <= ScreenFrameAbs.Width && y >= 0 && y <= ScreenFrameAbs.Height) {
                Terminal.Put(ScreenFrameAbs.X + x, ScreenFrameAbs.Y + y, code);
            }
        }

        public void Print(int x, int y, ContentAlignment alignment, string text, params object[] args)
        {
            if (x >= 0 && x <= ScreenFrameAbs.Width && y >= 0 && y <= ScreenFrameAbs.Height)
            {
                Terminal.Print(ScreenFrameAbs.X + x, ScreenFrameAbs.Y + y, alignment, text, args);
            }
        }
    }
}
