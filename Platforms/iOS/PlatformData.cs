using FSLibrary;
using Foundation;
using System;
using System.IO;
using System.Threading.Tasks;
using UIKit;
using MauiAppModel = Microsoft.Maui.ApplicationModel;
using MauiDevices = Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;

namespace Summatic.iOS
{
    public class PlatformData : IPlatformData
    {
        public OS OS => OS.IOS;

        public ISignal<float> DpiScaling => Signal.constant((float)UIScreen.MainScreen.Scale);

        public Func<Task> OpenAppInStore => OpenAppInStoreHelper;

        public FileLoaderSaver FileLoaderSaver => FileLoaderSaver.OpenOnly(OpenFile);

        public Task<string> GetSkiaTypefacesFromAssetFont() =>
            Task.Run(() =>
            {
                return "GetSkiaTypefacesFromAssetFont finished";
            });
        private static async Task OpenAppInStoreHelper() =>
            await UIApplication.SharedApplication.OpenUrlAsync(new NSUrl(""), new UIApplicationOpenUrlOptions());

        public Task CallOnRenderThread(Action value) => MauiAppModel.MainThread.InvokeOnMainThreadAsync(value);

        public Task<a> CallOnRenderThread<a>(Func<a> value) => MauiAppModel.MainThread.InvokeOnMainThreadAsync(value);

        public Task CallTaskOnRenderThread(Func<Task> value) => MauiAppModel.MainThread.InvokeOnMainThreadAsync(value);

        public Task<a> CallTaskOnRenderThread<a>(Func<Task<a>> value) => MauiAppModel.MainThread.InvokeOnMainThreadAsync(value);

        public DeviceType DeviceType
        {
            get
            {
                if (MauiDevices.DeviceInfo.Idiom == MauiDevices.DeviceIdiom.Phone)
                {
                    return DeviceType.Phone;
                }
                else if (MauiDevices.DeviceInfo.Idiom == MauiDevices.DeviceIdiom.Tablet)
                {
                    return DeviceType.Tablet;
                }
                else if (MauiDevices.DeviceInfo.Idiom == MauiDevices.DeviceIdiom.Desktop)
                {
                    return DeviceType.Desktop;
                }
                else
                {
                    throw new Exception("Unexpected device type");
                }
            }
        }

        public Task Share(Sharable sharable) =>
            MauiAppModel.DataTransfer.Share.Default.RequestAsync(new MauiAppModel.DataTransfer.ShareTextRequest
            {
                Text = sharable.Text,
                Title = "Summatic",
                Subject = "Share from Summatic",
                Uri = sharable.Uri
            });

        public Task Open(Uri value) => MauiAppModel.Launcher.Default.OpenAsync(value);

        public Func<FileSaveData, Task<bool>> OpenFile =>
            async fsd =>
            {
                string file = "";
                await MauiAppModel.Launcher.Default.OpenAsync(new MauiAppModel.OpenFileRequest("title", new ReadOnlyFile(file)));
                return true;
            };
    }
}