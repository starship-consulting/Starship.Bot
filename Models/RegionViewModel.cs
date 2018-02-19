using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Starship.Bot.Interfaces;
using Starship.Core.Events;
using Starship.Core.Events.Standard;
using Starship.Core.Extensions;
using Starship.Core.Reflection;
using Starship.Imaging;
using Starship.Win32.Extensions;
using Starship.Win32.Presentation;
using Point = System.Windows.Point;
using Region = Starship.Bot.Data.Region;

namespace Starship.Bot.Models {
    public class RegionViewModel {

        public RegionViewModel(Region region) {
            Rectangle = Automapper.Map<RectangleElement>(region);
            Label = new TextElement();
            Id = region.Id;
            Name = region.Name;
            Image = region.Thumbnail;
            
            UpdateLabel();
        }

        public RegionViewModel(Point position) : this(new Region(position)) {
        }

        public Point GetCenter() {
            return Rectangle.GetCenter();
        }
        
        public void Commit() {
            EventStream.Publish(new EntityModified(GetRegion()));
            //EventStream.Publish(new FileModified(Id + ".png", Image.ToBytes(ImageFormat.Png)));
        }

        public void Scan(Image source) {
            OCR = string.Empty;
            
            var capture = CaptureImage(source);

            if (Image == null) {
                Image = capture;
                EventStream.Publish(new FileModified(Id + ".png", Image.ToBytes(ImageFormat.Png)));
            }

            var results = ImageScanner.Compare(Image, capture, 0.9f);
            Exists = results.Any(each => each.Similarity >= 0.95f && Rectangle.Equals(each.Rectangle));
            
            /*if (Exists) {
                Rectangle.SetBorderColor(1, 0, 1, 0);
            }
            else {
                Rectangle.SetBorderColor(1, 1, 0, 0);
            }*/

            if (Rectangle.Width > 0 && Rectangle.Height > 0) {
                OCR = ImageScanner.ReadText(new Bitmap(Image)).Replace("\n", "");
            }

            UpdateLabel();
        }

        private Image CaptureImage(Image source) {
            if (Rectangle.Width > 0 && Rectangle.Height > 0) {
                return source.Crop(new Rectangle(Rectangle.X + 2, Rectangle.Y + 2, Rectangle.Width - 4, Rectangle.Height - 4));
            }

            return new Bitmap(1, 1);
        }

        private Region GetRegion() {
            return new Region(Rectangle) {
                Id = Id,
                Name = Name,
                OCR = OCR
            };
        }

        public void AddToUI(IsOverlay ui) {
            ui.Add(Rectangle, Label);
        }

        public void ApplyDrag(Point origin, Point target) {
            Rectangle.Edit(() => {
                if (target.X < origin.X) {
                    Rectangle.X = (int) target.X;
                    Rectangle.Width = (int) (origin.X - target.X);
                }
                else {
                    Rectangle.X = (int) origin.X;
                    Rectangle.Width = (int) (target.X - origin.X);
                }

                if (target.Y < origin.Y) {
                    Rectangle.Y = (int) target.Y;
                    Rectangle.Height = (int) (origin.Y - target.Y);
                }
                else {
                    Rectangle.Y = (int) origin.Y;
                    Rectangle.Height = (int) (target.Y - origin.Y);
                }
            });

            UpdateLabel();
        }

        private void UpdateLabel() {
            Label.Edit(() => {
                Label.Text = ToString();
                Label.X = Rectangle.X;

                if (Rectangle.Y <= 20) {
                    Label.Y = Rectangle.Y + Rectangle.Height;
                }
                else {
                    Label.Y = Rectangle.Y - 20;
                }
            });
        }

        public override string ToString() {
            var ocr = OCR.IsEmpty() ? string.Empty : "'" + OCR + "'";
            return $"{Rectangle.X} {Rectangle.Y} {Rectangle.Width} {Rectangle.Height} {ocr}";
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool Exists { get; set; }

        private Image Image { get; set; }

        private string OCR { get; set; }

        private RectangleElement Rectangle { get; set; }

        private TextElement Label { get; set; }
    }
}