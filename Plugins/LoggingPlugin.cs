using System;
using System.Diagnostics;
using Starship.Core.Events;
using Starship.Core.Plugins;

namespace Starship.Bot.Plugins {
    public class LoggingPlugin : Plugin {

        public override void Ready() {
            base.Ready();

            //EventStream.On(EventTriggered);
        }

        private void EventTriggered(object e) {
            //Debug.WriteLine(DateTime.Now.ToLongTimeString() + ": " + e.GetType().Name);
        }
    }
}