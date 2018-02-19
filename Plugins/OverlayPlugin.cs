using System;
using System.Windows.Media;
using Starship.Bot.Events;
using Starship.Core.ChangeTracking;
using Starship.Win32;

namespace Starship.Bot.Plugins {
    public class OverlayPlugin : GamePlugin {
        
        protected override void OnGameLoaded(GameLoaded e) {
            SetEditMode(true);

            UI.SetParent(GameWindow.Handle);

            GameWindow.BringToFront();
            UpdateState(GameWindow);

            WindowTracker = new ChangeTracker<WindowInstance>(GameWindow);
            WindowTracker.Changed += OnWindowChanged;
            WindowTracker.StartPolling(TimeSpan.FromSeconds(1), GameWindow.Update);
        }

        private void SetEditMode(bool active) {
            if (active) {
                UI.SetBackgroundColor(Color.FromScRgb(1, 0, 0, 0));
            }
            else {
                UI.SetBackgroundColor(Color.FromScRgb(0, 0, 0, 0));
            }
        }

        private void OnWindowChanged(WindowInstance window, ChangeTrackerState state) {
            UpdateState(window);
        }

        private void UpdateState(WindowInstance window) {
            //if (window.State == WindowStates.Normal || window.State == WindowStates.Maximized) {
                UI.SetSize(window.Width, window.Height);
                UI.SetPosition(window.X, window.Y);
            //}
        }
        
        private ChangeTracker<WindowInstance> WindowTracker { get; set; }
    }
}