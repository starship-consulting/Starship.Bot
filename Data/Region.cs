using System;
using System.Drawing;
using Starship.Core.Data.Attributes;
using Starship.Core.Reflection;
using Starship.Win32.Presentation;
using Point = System.Windows.Point;

namespace Starship.Bot.Data {
    public class Region {

        public Region() {
            Id = Guid.NewGuid();
            Name = "NewRegion";
        }

        public Region(int x, int y) : this() {
            X = x;
            Y = y;
        }

        public Region(Point position) : this((int) position.X, (int) position.Y) {
        }

        public Region(RectangleElement element) : this() {
            Automap.Map(element, this);
        }

        [PrimaryKey]
        public Guid Id { get; set; }
        
        public string Parent { get; set; }

        public string Name { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public string OCR { get; set; }
        
        public int Width { get; set; }

        public int Height { get; set; }

        [NonSerialized]
        public Image Thumbnail;
    }
}