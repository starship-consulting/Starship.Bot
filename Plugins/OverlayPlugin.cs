using System;
using Starship.Bot.Events;
using Starship.Core.ChangeTracking;
using Starship.Win32;

namespace Starship.Bot.Plugins {
    public class OverlayPlugin : GamePlugin {
        
        protected override void OnGameLoaded(GameLoaded e) {
            SetEditMode(EditMode);

            Overlay.SetParent(Window.Handle);

            Window.BringToFront();
            UpdateState();

            WindowTracker = new ChangeTracker<WindowInstance>(Window);
            WindowTracker.Changed += OnWindowChanged;
            WindowTracker.StartPolling(TimeSpan.FromSeconds(1), Window.Update);

            On<EditModeChanged>(mode => SetEditMode(mode.Enabled));
        }

        protected override void Stopped() {
            base.Stopped();

            WindowTracker.Dispose();
            WindowTracker = null;
        }

        private void SetEditMode(bool active) {
            EditMode = active;

            if (EditMode) {
                Overlay.MakeOpaque();
            }
            else {
                Overlay.MakeTransparent();
            }
        }

        private void OnWindowChanged(WindowInstance window, ChangeTrackerState state) {
            UpdateState();
        }

        private void UpdateState() {
            //if (window.State == WindowStates.Normal || window.State == WindowStates.Maximized) {
                Overlay.SetSize(Window.Width, Window.Height);
                Overlay.SetPosition(Window.X, Window.Y);
            //}
        }
        
        public bool EditMode { get; set; }

        private ChangeTracker<WindowInstance> WindowTracker { get; set; }
    }
}