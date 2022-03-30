using Library.Controls;
using Library.Effects;
using Library.Hosting;
using Microsoft.Maui.Controls.Compatibility.Hosting;

namespace Library.Sample
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureLibrary()
                /*
                .ConfigureEffects(effects =>
                {
                    effects.Add(typeof(FocusRoutingEffect), typeof(PlatformFocusPlatformEffect));
                })
                .ConfigureMauiHandlers(handlers =>
                 {
#if __ANDROID__
                     handlers.AddCompatibilityRenderer(typeof(CustomEntry), typeof(Renderers.Android.CustomEntryRenderer));
#endif
                 })
                */;

            return builder.Build();
        }
    }
}