using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Remoting;
using EasyHook;
using Starship.Bot.Core;
using Starship.Bot.Events;
using Starship.Core.Extensions;
using Starship.Core.Processing;
using Starship.Injection;

namespace Starship.Bot.Plugins {
    public class ImageCapturePlugin : GamePlugin {

        public ImageCapturePlugin() {
            LastFrameUpdate = DateTime.Now;
        }

        protected override void OnGameLoaded(GameLoaded e) {
            base.OnGameLoaded(e);
      
            switch (Type.ToLower()) {
                case "opengl":
                    BeginOpenGLImageCapture();
                    break;
                default:
                    BeginDefaultImageCapture();
                    break;
            }
        }

        protected override void Stopped() {
            base.Stopped();

            if (Proxy == null) {
                return;
            }

            Proxy.Close();
            Proxy = null;
        }

        private void BeginOpenGLImageCapture() {
            string channel = null;
            var path = Environment.CurrentDirectory + @"\Starship.Injection.dll";
            
            ImageStreamProxy.DebugReceived += OnDebugReceived;
            ImageStreamProxy.ClientCallbackProxyGranted += OnClientCallbackProxyGranted;
            ImageStreamProxy.ImageDataReceived += OnImageDataReceived;
            
            ImageStreamProxy.Settings = new ImageCaptureSettings {
                Width = GameBot.Window.Width,
                Height = GameBot.Window.Height,
                Interval = 250
            };

            RemoteHooking.IpcCreateServer<ImageStreamProxy>(ref channel, WellKnownObjectMode.Singleton);
            RemoteHooking.Inject(GameBot.Process.GetProcess().Id, path, path, channel);
        }

        private void OnDebugReceived(string message) {
            Debug.WriteLine(message);
        }

        private void OnClientCallbackProxyGranted(ClientCallbackProxy proxy) {
            Proxy = proxy;
        }

        private void OnImageDataReceived(byte[] data) {
            using (var stream = new MemoryStream(data)) {
                var image = Image.FromStream(stream);

                if (!HasReceivedImage) {
                    HasReceivedImage = true;
                    //image.OpenInPaint();
                }

                IncrementFrame();
                BroadcastImage(image);
            }
        }

        private void BeginDefaultImageCapture() {
            Worker = new Worker(Interval, OnInterval);
            Worker.Start();
        }
        
        private void OnInterval() {
            BroadcastImage(GameBot.Window.CaptureImage());
        }

        private void BroadcastImage(Image image) {
            
            /*if (Frames == 60) {
                var elapsed = DateTime.Now - LastFrameUpdate;
                FrameRate = Frames / Convert.ToInt32(elapsed.TotalSeconds);
            }*/

            //Publish(new ImageCaptured(image));
        }

        private void IncrementFrame() {
            Frames += 1;

            if(LastFrameUpdate.HasElapsed(TimeSpan.FromSeconds(1))) {
                Publish(new FrameRateUpdated { FPS = Frames });
                Frames = 0;
                LastFrameUpdate = DateTime.Now;
            }
        }

        public string Type { get; set; }
        
        private DateTime LastFrameUpdate { get; set; }

        private int Frames { get; set; }

        private TimeSpan Interval = TimeSpan.FromMilliseconds(500);

        private Worker Worker { get; set; }

        private ClientCallbackProxy Proxy { get; set; }

        private bool HasReceivedImage { get; set; }
    }
}