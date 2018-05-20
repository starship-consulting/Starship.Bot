using System;

namespace Starship.Bot.Events {
    public class EditModeChanged {

        public EditModeChanged() {
        }

        public EditModeChanged(bool enabled) {
            Enabled = enabled;
        }

        public bool Enabled { get; set; }
    }
}