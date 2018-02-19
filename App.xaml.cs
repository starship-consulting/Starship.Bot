using System;
using System.Windows;
//using CefSharp;

namespace Starship.Bot {
    public partial class App : Application {
        public App() {
            //AppDomain.CurrentDomain.AssemblyResolve += Resolver;
            //InitializeCefSharp();
        }

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            var window = new Windows.MainWindow();
            window.Closed += OnMainWindowClosed;
            window.Show();
        }

        private void OnMainWindowClosed(object sender, EventArgs e) {
            Shutdown();
        }

        /*[MethodImpl(MethodImplOptions.NoInlining)]
        private static void InitializeCefSharp() {
            var settings = new CefSettings();
            settings.BrowserSubprocessPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                Environment.Is64BitProcess ? "x64" : "x86",
                "CefSharp.BrowserSubprocess.exe");

            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
        }
        
        private static Assembly Resolver(object sender, ResolveEventArgs args) {
            if (args.Name.StartsWith("CefSharp")) {
                string assemblyName = args.Name.Split(new[] {','}, 2)[0] + ".dll";
                string archSpecificPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                    Environment.Is64BitProcess ? "x64" : "x86",
                    assemblyName);

                return File.Exists(archSpecificPath) ? Assembly.LoadFile(archSpecificPath) : null;
            }

            return null;
        }*/
    }
}