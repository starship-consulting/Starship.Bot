using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Starship.Win32.Presentation;

namespace Starship.Bot.UserControls {
    public partial class RegionControl : UserControl {

        public RegionControl() {
            InitializeComponent();
        }

        public RegionControl(RectangleElement element) : this() {
            DataContext = element;
        }

        protected override void OnMouseEnter(MouseEventArgs e) {
            base.OnMouseEnter(e);

            var rectangle = DataContext as RectangleElement;

            Label = new TextBlock();
            Label.Text = rectangle.Name;
            Label.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            Canvas.SetLeft(Label, rectangle.X + 5);
            Canvas.SetTop(Label, rectangle.Y + 5);

            Canvas.Children.Add(Label);
        }

        protected override void OnMouseLeave(MouseEventArgs e) {
            base.OnMouseLeave(e);

            Canvas.Children.Remove(Label);
            Label = null;
        }

        private TextBlock Label { get; set; }
    }
}