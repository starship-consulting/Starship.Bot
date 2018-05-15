using System;
using System.Windows.Forms;
using Starship.Win32.Input;

namespace Starship.Bot.Plugins {
    public class HotkeyPlugin : GamePlugin {

        public override void Ready() {
            KeyboardListener.KeyPressed += OnKeyPressed;
            KeyboardListener.Start();
        }

        private void OnKeyPressed(KeyPressEventArgs e) {

        }
    }
}