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