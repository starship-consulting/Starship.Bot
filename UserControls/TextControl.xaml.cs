using System.Windows.Controls;
using Starship.Win32.Presentation;

namespace Starship.Bot.UserControls {
    public partial class TextControl : UserControl {
        public TextControl() {
            InitializeComponent();
        }

        public TextControl(TextElement element) : this() {
            DataContext = element;
        }
    }
}