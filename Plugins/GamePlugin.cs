using System;
using Starship.Bot.Core;
using Starship.Bot.Events;
using Starship.Bot.Interfaces;
using Starship.Core.Plugins;
using Starship.Win32;

namespace Starship.Bot.Plugins {
    public class GamePlugin : Plugin {

        public override void Ready() {
            On<GameLoaded>(OnGameLoaded);
        }
        
        public void With<T>(Action<T> action) where T : Plugin {
            GameBot.Plugins.With(action);
        }

        protected virtual void OnGameLoaded(GameLoaded e) {
        }

        protected IsOverlay Overlay => GameBot.Overlay;

        protected WindowInstance Window => GameBot.Window;

        protected WindowsProcess Process => GameBot.Process;
    }
}