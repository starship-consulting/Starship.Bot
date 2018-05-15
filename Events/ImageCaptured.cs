
using System.Drawing;

namespace Starship.Bot.Events {
    public class ImageCaptured {

        public ImageCaptured() {
        }

        public ImageCaptured(Image image) {
            Image = image;
        }

        public Image Image { get; set; }
    }
}