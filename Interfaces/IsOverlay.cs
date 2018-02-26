using System;
using Starship.Bot.Models;
using Starship.Win32.Presentation;
using Color = System.Windows.Media.Color;

namespace Starship.Bot.Interfaces {
    public interface IsOverlay {
        void MakeOpaque();
        void MakeTransparent();
        void Add(params VisualElement[] element);
        void Remove(params VisualElement[] element);
        void SetBackgroundColor(Color color);
        void SetSize(double width, double height);
        void SetPosition(double x, double y);
        void SetParent(IntPtr parentHandle);
        event Action<MouseState> MouseStateChanged;
    }
}