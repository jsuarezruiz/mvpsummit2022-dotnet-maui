using Android.App;
using Android.Runtime;
using Library.Handlers;

namespace Library.Sample
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
            /*
            CustomEntryHandler.CustomEntryMapper.Add("RemoveBorder", (h, w) =>
            {
                h.PlatformView.Background = null;
            });
            */
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}