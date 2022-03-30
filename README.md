# MVP Summit 2022 - .NET MAUI samples

Hello Microsoft MVPs!

![Microsoft MVP](images/mvp-summit.jpg)

Here you can find demos to learn how to port code and libraries from Xamarin.Forms to .NET MAUI.

_Ready?_.

## Port Xamarin.Forms libraries (or plugins) to .NET MAUI

Let's see how to port code and the implications in each case.

### Port Xamarin.Forms Behaviors to .NET MAUI

Behaviors let you add functionality to user interface controls without having to subclass them. Instead, the functionality is implemented in a behavior class and attached to the control as if it was part of the control itself.

In .NET MAUI the same concept works exactly the same, being able to reuse all the code. 

Let's see an example.

Xamarin.Forms

```
using Xamarin.Forms;

namespace Behaviors
{
    public class NumericValidationBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            bool isValid = double.TryParse(args.NewTextValue, out double result);
            ((Entry)sender).TextColor = isValid ? Color.Default : Color.Red;
        }
    }
}
```
.NET MAUI

```
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Behaviors
{
    public class NumericValidationBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            bool isValid = double.TryParse(args.NewTextValue, out double result);
            ((Entry)sender).TextColor = isValid ? Colors.Black : Colors.Red;
        }
    }
}
```

_What is the difference?_. The code is exactly the same except for one detail, namespaces.

Xamarin.Forms namespace changes to **Microsoft.Maui.Controls** namespace. 

On the other hand, all the basic types such as: Color, Rectangle or Point have become available in **Microsoft.Maui.Graphics**. For that reason, and because using colors in this Behavior we also need this new namespace. 

### Port Xamarin.Forms Converters to .NET MAUI

Data bindings usually transfer data from a source property to a target property, and in some cases from the target property to the source property. This transfer is straightforward when the source and target properties are of the same type, or when one type can be converted to the other type through an implicit conversion. When that is not the case, a **type conversion** must take place.

Suppose you want to define a data binding where the source property is of type int but the target property is a bool. You want this data binding to produce a false value when the integer source is equal to 0, and true otherwise.

You can do this with a class that implements the **IValueConverter** interface.

Like other Xamarin.Forms concepts, **Converters** can be reused in  .NET MAUI without require code changes.

Let's see an example. 

Xamarin.Forms

```
using System;
using System.Globalization;
using Xamarin.Forms;

namespace Converters
{
    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value != 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 1 : 0;
        }
    }
}
```
.NET MAUI

```
using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace Converters
{
    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value != 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 1 : 0;
        }
    }
}
```

_What is the difference?_. The code is exactly the same except for one detail, namespaces.

Xamarin.Forms namespace changes to **Microsoft.Maui.Controls** namespace. 

### Port Xamarin.Forms Effects to .NET MAUI

Xamarin.Forms user interfaces are rendered using the native controls of the target platform, allowing Xamarin.Forms applications to retain the appropriate look and feel for each platform. Effects allow the native controls on each platform to be customized without having to resort to a custom renderer implementation.

With the new extensibility possibilities of .NET MAUI Handlers, the use of Effects will probably decrease but even so, there is the same concept and porting effects is simple.

## Xamarin.Forms

The process for creating an effect in each platform-specific project is as follows:

1. Create a subclass of the PlatformEffect class.
2. Override the OnAttached method and write logic to customize the control.
3. Override the OnDetached method and write logic to clean up the control customization, if required.
4. Add a ResolutionGroupName attribute to the effect class. This attribute sets a company wide namespace for effects, preventing collisions with other effects with the same name. Note that this attribute can only be applied once per project.
5. Add an ExportEffect attribute to the effect class. This attribute registers the effect with a unique ID that's used by Xamarin.Forms, along with the group name, to locate the effect prior to applying it to a control. The attribute takes two parameters – the type name of the effect, and a unique string that will be used to locate the effect prior to applying it to a control.


### Creating the Effect on Each Platform

```
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("MyCompany")]
[assembly: ExportEffect(typeof(EffectsDemo.Droid.FocusEffect), nameof(EffectsDemo.Droid.FocusEffect))]
namespace EffectsDemo.Droid
{
    public class FocusEffect : PlatformEffect
    {
        Android.Graphics.Color originalBackgroundColor = new Android.Graphics.Color(0, 0, 0, 0);
        Android.Graphics.Color backgroundColor;

        protected override void OnAttached()
        {
            try
            {
                backgroundColor = Android.Graphics.Color.LightGreen;
                Control.SetBackgroundColor(backgroundColor);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {
        }

        protected override void OnElementPropertyChanged(System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            try
            {
                if (args.PropertyName == "IsFocused")
                {
                    if (((Android.Graphics.Drawables.ColorDrawable)Control.Background).Color == backgroundColor)
                    {
                        Control.SetBackgroundColor(originalBackgroundColor);
                    }
                    else
                    {
                        Control.SetBackgroundColor(backgroundColor);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }
    }
}
```

The OnAttached method calls the SetBackgroundColor method to set the background color of the control to light green, and also stores this color in a field. This functionality is wrapped in a try/catch block in case the control the effect is attached to does not have a SetBackgroundColor property. No implementation is provided by the OnDetached method because no cleanup is necessary.

The OnElementPropertyChanged override responds to bindable property changes on the Xamarin.Forms control. When the IsFocused property changes, the background color of the control is changed to white if the control has focus, otherwise it's changed to light green. This functionality is wrapped in a try/catch block in case the control the effect is attached to does not have a BackgroundColor property.

### Consuming the Effect in XAML

The following XAML code example shows an Entry control to which the FocusEffect is attached:

```
<Entry Text="Effect attached to an Entry" ...>
    <Entry.Effects>
        <local:FocusEffect />
    </Entry.Effects>
    ...
</Entry>
```

The FocusEffect class subclasses the RoutingEffect class, which represents a platform-independent effect that wraps an inner effect that is usually platform-specific. The FocusEffect class calls the base class constructor, passing in a parameter consisting of a concatenation of the resolution group name (specified using the ResolutionGroupName attribute on the effect class), and the unique ID that was specified using the ExportEffect attribute on the effect class. Therefore, when the Entry is initialized at runtime, a new instance of the Effects.FocusEffect is added to the control's Effects collection.

```
public class FocusEffect : RoutingEffect
{
    public FocusEffect () : base ($"Effects.{nameof(FocusEffect)}")
    {
    }
}
```
## .NET MAUI

We can reuse all the effects code by simply changing the namespace from Xamarin.Forms to Microsoft.Maui.Controls. 

```
using Microsoft.Maui.Controls;
using System;
using System.ComponentModel;

namespace Effects.Effects
{
    public class FocusRoutingEffect : RoutingEffect
    {

    }

#if WINDOWS
	public class FocusPlatformEffect : Microsoft.Maui.Controls.Compatibility.Platform.UWP.PlatformEffect
    {
		public FocusPlatformEffect() : base()
		{
		}

		protected override void OnAttached()
		{
			try
			{
				(Control as Microsoft.UI.Xaml.Controls.Control).Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Cyan);
				(Control as Microsoft.Maui.MauiTextBox).BackgroundFocusBrush = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
			}
			catch (Exception ex)
			{
                System.Diagnostics.Debug.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
			}

		}

		protected override void OnDetached()
		{
		}
	}
#elif __ANDROID__
    public class FocusPlatformEffect : Microsoft.Maui.Controls.Compatibility.Platform.Android.PlatformEffect
    {
        Android.Graphics.Color originalBackgroundColor = new Android.Graphics.Color(0, 0, 0, 0);
        Android.Graphics.Color backgroundColor;

        protected override void OnAttached()
        {
            try
            {
                backgroundColor = Android.Graphics.Color.LightGreen;
                Control.SetBackgroundColor(backgroundColor);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            try
            {
                if (args.PropertyName == "IsFocused")
                {
                    if (((Android.Graphics.Drawables.ColorDrawable)Control.Background).Color == backgroundColor)
                    {
                        Control.SetBackgroundColor(originalBackgroundColor);
                    }
                    else
                    {
                        Control.SetBackgroundColor(backgroundColor);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }
    }
#elif __IOS__
	public class FocusPlatformEffect : Microsoft.Maui.Controls.Compatibility.Platform.iOS.PlatformEffect
    {
		UIKit.UIColor backgroundColor;

		protected override void OnAttached()
		{
			try
			{
				Control.BackgroundColor = backgroundColor = UIKit.UIColor.FromRGB(204, 153, 255);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
			}
		}

		protected override void OnDetached()
		{
		}

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);

			try
			{
				if (args.PropertyName == "IsFocused")
				{
					if (Control.BackgroundColor == backgroundColor)
					{
						Control.BackgroundColor = UIKit.UIColor.White;
					}
					else
					{
						Control.BackgroundColor = backgroundColor;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
			}
		}
	}
#endif
}
```

Where if we have an important change is when registering the effect. 

In Xamarin.Forms we use the  ExportAttribure to register the effect with a unique ID that's used by Xamarin.Forms, along with the group name, to locate the effect prior to applying it to a control. As with Renderers, this attribute makes use of Assembly Scanning which is slow and penalizes performance. 

In .NET MAUI we use the AppHostBuilder and **ConfigureEffects** to register effects. 

```
appBuilder
    .ConfigureEffects(effects =>
    {
        effects.Add<FocusRoutingEffect, FocusPlatformEffect>();
    });
```