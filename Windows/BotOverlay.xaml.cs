using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Starship.Bot.Interfaces;
using Starship.Bot.Models;
using Starship.Bot.UserControls;
using Starship.Core.Utility;
using Starship.Win32.Extensions;
using Starship.Win32.Presentation;

namespace Starship.Bot.Windows {
    public partial class BotOverlay : Window, IsOverlay {

        public BotOverlay() {
            InitializeComponent();
            MakeTransparent();

            Elements = new List<VisualElement>();
            Mouse = new MouseState();

            ControlBuilder = new WorkFlow()
                .On<RectangleElement>(element => {
                    MainWindow.Instance.Add(element);
                    return new RegionControl(element);
                })
                //.On<TextElement>(element => new TextControl(element))
                .Finally<FrameworkElement>(element => { MainCanvas.Children.Add(element); });
        }

        public void MakeTransparent() {
            SetBackgroundColor(Color.FromScRgb(0, 0, 0, 0));
        }

        public void MakeOpaque() {
            SetBackgroundColor(Color.FromScRgb(1, 0, 0, 0));
        }
        
        public void Add(params VisualElement[] elements) {
            this.UI(() => { elements.ToList().ForEach(each => ControlBuilder.Process(each)); });
        }

        public void Remove(params VisualElement[] element) {
        }
        
        public void SetBackgroundColor(Color color) {
            this.UI(() => { BackgroundColor.Color = color; });
        }

        public void SetSize(double width, double height) {
            this.SetControlSize(width, height);
        }

        public void SetPosition(double x, double y) {
            this.SetWindowPosition(x, y);
        }

        public void SetParent(IntPtr parentHandle) {
            this.SetWindowParent(parentHandle);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            UpdateMouseState(e);
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e) {
            UpdateMouseState(e);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e) {
            UpdateMousePosition(e);
        }

        private void UpdateMouseState(MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left) {
                Mouse.IsLeftButtonDown = e.ButtonState == MouseButtonState.Pressed;
            }
            else if (e.ChangedButton == MouseButton.Right) {
                Mouse.IsRightButtonDown = e.ButtonState == MouseButtonState.Pressed;
            }

            UpdateMousePosition(e);
        }

        private void UpdateMousePosition(MouseEventArgs e) {
            if (MouseStateChanged != null) {
                Mouse.Position = e.GetPosition(this);
                MouseStateChanged(Mouse);
            }
        }

        public event Action<MouseState> MouseStateChanged;

        private MouseState Mouse { get; set; }

        private List<VisualElement> Elements { get; set; }

        private WorkFlow ControlBuilder { get; set; }
    }
}