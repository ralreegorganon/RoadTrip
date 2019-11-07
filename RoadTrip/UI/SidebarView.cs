namespace RoadTrip.UI
{
    public class SidebarView : View
    {
        public SidebarView()
        {
            BorderStyle = new BorderStyle {
                NW = '╒',
                SW = '╘',
                NE = '╕',
                SE = '╛',
                N = '╌',
                S = '╌',
                W = '╎',
                E = '╎'
            };

            Title = "Sidebar";
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
