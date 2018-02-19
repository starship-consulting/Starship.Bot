using System.Windows;

namespace Starship.Bot.Models {
    public class MouseState {
        public MouseState() {
        }

        public MouseState(int x, int y) {
            Position = new Point(x, y);
        }

        public Point Position { get; set; }

        public bool IsLeftButtonDown { get; set; }

        public bool IsRightButtonDown { get; set; }
    }
}