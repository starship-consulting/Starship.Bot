using System;
using Starship.Bot.Core;
using Starship.Bot.Events;
using Starship.Bot.Interfaces;
using Starship.Core.Plugins;
using Starship.Win32;

namespace Starship.Bot.Plugins {
    public class GamePlugin : Plugin {

        public override void Ready() {
            On<GameLoaded>(_OnGameLoaded);
        }

        private void _OnGameLoaded(GameLoaded e) {
            GameProcess = e.Process;
            GameWindow = e.Process.GetWindow();
            GameWindow.Update();

            OnGameLoaded(e);
        }

        protected void With<T>(Action<T> action) where T : Plugin {
            GameBot.Plugins.With(action);
        }

        protected virtual void OnGameLoaded(GameLoaded e) {
        }

        protected IsOverlay UI => GameBot.UI;

        protected WindowsProcess GameProcess { get; set; }

        protected WindowInstance GameWindow { get; set; }
    }
}