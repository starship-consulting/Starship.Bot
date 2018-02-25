using Starship.Bot.Core;
using Starship.Bot.Events;
using Starship.Core.Plugins;
using Starship.Win32;

namespace Starship.Bot.Plugins {
    public class GameLoaderPlugin : Plugin {

        protected override void Run() {
            Process = new WindowsProcess(ProcessName);

            var process = Process.GetProcess();

            if (process != null) {
                GameBot.Process = Process;
                GameBot.Window = Process.GetWindow();
                GameBot.Window.Update();

                Publish(new GameLoaded());
            }
        }

        public string ProcessName { get; set; }

        private WindowsProcess Process { get; set; }
    }
}