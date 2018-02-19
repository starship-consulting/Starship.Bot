using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Remoting;
using EasyHook;
using Starship.Bot.Events;
using Starship.Core.Processing;
using Starship.Injection;

namespace Starship.Bot.Plugins {
    public class ImageCapturePlugin : GamePlugin {

        protected override void OnGameLoaded(GameLoaded e) {
            base.OnGameLoaded(e);

            return;

            switch (Type.ToLower()) {
                case "opengl":
                    BeginOpenGLImageCapture();
                    break;
                default:
                    BeginDefaultImageCapture();
                    break;
            }
        }

        private void BeginOpenGLImageCapture() {
            string channel = null;
            var path = Environment.CurrentDirectory + @"\Starship.Injection.dll";
            
            ImageStreamProxy.DebugReceived += OnDebugReceived;
            ImageStreamProxy.ClientCallbackProxyGranted += OnClientCallbackProxyGranted;
            ImageStreamProxy.ImageDataReceived += OnImageDataReceived;
            
            ImageStreamProxy.Settings = new ImageCaptureSettings {
                Width = GameWindow.Width,
                Height = GameWindow.Height,
                Interval = 250
            };

            RemoteHooking.IpcCreateServer<ImageStreamProxy>(ref channel, WellKnownObjectMode.Singleton);
            RemoteHooking.Inject(GameProcess.GetProcess().Id, path, path, channel);
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

                BroadcastImage(image);
            }
        }

        private void Close() {
            if (Proxy == null) {
                return;
            }

            Proxy.Close();
            Proxy = null;
        }

        private void BeginDefaultImageCapture() {
            Worker = new Worker(Interval, OnInterval);
            Worker.Start();
        }
        
        private void OnInterval() {
            BroadcastImage(GameWindow.CaptureImage());
        }

        private void BroadcastImage(Image image) {
            /*Frames += 1;
            
            if (Frames == 60) {
                var elapsed = DateTime.Now - LastFrameUpdate;
                FrameRate = Frames / Convert.ToInt32(elapsed.TotalSeconds);
            }*/

            Publish(new ImageCaptured(image));
        }

        public string Type { get; set; }

        private int FrameRate { get; set; }

        private DateTime LastFrameUpdate { get; set; }

        private int Frames { get; set; }

        private TimeSpan Interval = TimeSpan.FromMilliseconds(500);

        private Worker Worker { get; set; }

        private ClientCallbackProxy Proxy { get; set; }

        private bool HasReceivedImage { get; set; }
    }
}