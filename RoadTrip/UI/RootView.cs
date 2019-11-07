using System.Drawing;
using BearLib;

namespace RoadTrip.UI
{
    public class RootView : View
    {
        public RootView(MapView mapView, SidebarView sidebarView)
        {
            MapView = mapView;
            SidebarView = sidebarView;

            Terminal.Open();
            Terminal.Set("window: title='road trip', resizeable=true, size=80x24;");
            Terminal.Set("text font: ./Cascadia.ttf, size=16x16");
            Terminal.Set("font: ./Topaz-8.ttf, size=8");
            Terminal.Set("output: vsync=false;");

            Resize();
        }

        public MapView MapView { get; }

        public SidebarView SidebarView { get; }

        public override void Draw()
        {
            Terminal.Clear();
            Terminal.Layer(1);
            MapView.Draw();
            SidebarView.Draw();
            Terminal.Refresh();
        }

        public void Resize()
        {
            var width = Terminal.State(Terminal.TK_WIDTH);
            var height = Terminal.State(Terminal.TK_HEIGHT);

            var cwidth = Terminal.State(Terminal.TK_CELL_WIDTH);
            var cheight = Terminal.State(Terminal.TK_CELL_HEIGHT);

            ScreenFrameAbs = new Rectangle(0, 0, width, height);
            MapView.ScreenFrameAbs = new Rectangle(ScreenFrameAbs.Left + 1, ScreenFrameAbs.Top + 1, ScreenFrameAbs.Right - 20, ScreenFrameAbs.Bottom - 2);
            SidebarView.ScreenFrameAbs = new Rectangle(MapView.ScreenFrameAbs.Right + 2, ScreenFrameAbs.Top + 1, ScreenFrameAbs.Width - MapView.ScreenFrameAbs.Width - 4, ScreenFrameAbs.Bottom - 2);
        }
    }
}
