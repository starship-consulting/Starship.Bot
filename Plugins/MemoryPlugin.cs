using System;
using Starship.Win32.Memory;

namespace Starship.Bot.Plugins {
    public class MemoryPlugin : GamePlugin {

        protected override void Run() {
            base.Run();

            Scanner = new MemoryScanner(Process.GetProcess());
            Scanner.Scan(1);
        }

        private MemoryScanner Scanner { get; set; }
    }
}