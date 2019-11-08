using System.Drawing;
using BearLib;

namespace RoadTrip.UI
{
    public abstract class View
    {
        public Rectangle ScreenFrameAbs { get; set; }

        protected BorderStyle? BorderStyle { get; set; }

        protected string? Title { get; set; }

        protected RunState CurrentRunState { get; set; }

        public virtual void Draw(RunState currentRunState)
        {
            CurrentRunState = currentRunState;

            DrawBorders();
            DrawTitle();
        }

        public void DrawBorders()
        {
            if (BorderStyle == null) {
                return;
            }

            Terminal.Color(Color.NavajoWhite);

            Terminal.Put(ScreenFrameAbs.X - 1, ScreenFrameAbs.Y - 1, BorderStyle.NW);
            Terminal.Put(ScreenFrameAbs.X + ScreenFrameAbs.Width, ScreenFrameAbs.Y - 1, BorderStyle.NE);
            Terminal.Put(ScreenFrameAbs.X - 1, ScreenFrameAbs.Y + ScreenFrameAbs.Height, BorderStyle.SW);
            Terminal.Put(ScreenFrameAbs.X + ScreenFrameAbs.Width, ScreenFrameAbs.Y + ScreenFrameAbs.Height, BorderStyle.SE);

            for (var dx = 0; dx < ScreenFrameAbs.Width; dx++) {
                Terminal.Put(ScreenFrameAbs.X + dx, ScreenFrameAbs.Y - 1, BorderStyle.N);
                Terminal.Put(ScreenFrameAbs.X + dx, ScreenFrameAbs.Y + ScreenFrameAbs.Height, BorderStyle.S);
            }

            for (var dy = 0; dy < ScreenFrameAbs.Height; dy++) {
                Terminal.Put(ScreenFrameAbs.X - 1, ScreenFrameAbs.Y + dy, BorderStyle.W);
                Terminal.Put(ScreenFrameAbs.X + ScreenFrameAbs.Width, ScreenFrameAbs.Y + dy, BorderStyle.E);
            }
        }

        public virtual void DrawTitle()
        {
            if (Title == null) {
                return;
            }

            Terminal.Color(Color.WhiteSmoke);

            Terminal.Print(ScreenFrameAbs.X, ScreenFrameAbs.Y - 1, ContentAlignment.TopLeft, Title);
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
