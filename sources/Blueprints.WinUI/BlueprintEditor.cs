using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using SkiaSharp;
using Windows.Storage;
using Windows.System;

namespace Blueprints.WinUI;

public sealed partial class BlueprintEditor : SKView, IBlueprintEditor
{
    public static readonly DependencyProperty XProperty = DependencyProperty.Register(nameof(X),
                                                                                      typeof(float),
                                                                                      typeof(BlueprintEditor),
                                                                                      new PropertyMetadata(0.0f));

    public static readonly DependencyProperty YProperty = DependencyProperty.Register(nameof(Y),
                                                                                      typeof(float),
                                                                                      typeof(BlueprintEditor),
                                                                                      new PropertyMetadata(0.0f));

    public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(nameof(Zoom),
                                                                                         typeof(float),
                                                                                         typeof(BlueprintEditor),
                                                                                         new PropertyMetadata(1.0f));

    public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register(nameof(FontFamily),
                                                                                               typeof(FontFamily),
                                                                                               typeof(BlueprintEditor),
                                                                                               new PropertyMetadata(FontFamily.XamlAutoFontFamily));

    public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register(nameof(Theme),
                                                                                          typeof(IBlueprintTheme),
                                                                                          typeof(BlueprintEditor),
                                                                                          new PropertyMetadata(DefaultBlueprintTheme.Instance));

    public static readonly DependencyProperty ElementsProperty = DependencyProperty.Register(nameof(Elements),
                                                                                             typeof(IEnumerable<Element>),
                                                                                             typeof(BlueprintEditor),
                                                                                             new PropertyMetadata(null));

    private bool isInvalidateScheduled;

    public BlueprintEditor()
    {
        UpdateTheme();
        ActualThemeChanged += (_, _) => UpdateTheme();

        BlueprintRenderer renderer = new(this);
        BlueprintController controller = new(this);

        Loaded += (_, _) => CompositionTarget.Rendering += Rendering;

        Unloaded += (_, _) => CompositionTarget.Rendering -= Rendering;

        Paint += (_, e) => renderer.Render(e);

        PointerMoved += (_, e) => controller.PointerMoved(PointerEventArgs(e));

        PointerPressed += (_, e) => controller.PointerPressed(PointerEventArgs(e));

        PointerReleased += (_, e) => controller.PointerReleased(PointerEventArgs(e));

        PointerWheelChanged += (_, e) => controller.PointerWheelChanged(PointerWheelEventArgs(e));

        void UpdateTheme()
        {
            Theme?.Mode = ActualTheme is ElementTheme.Light ? ThemeMode.Light : ThemeMode.Dark;

            Invalidate();
        }

        PointerEventArgs PointerEventArgs(PointerRoutedEventArgs e)
        {
            PointerPoint pointerPoint = e.GetCurrentPoint(this);

            Pointers pointers = Pointers.None;

            if (pointerPoint.Properties.IsLeftButtonPressed)
            {
                pointers |= Pointers.LeftButton;
            }

            if (pointerPoint.Properties.IsRightButtonPressed)
            {
                pointers |= Pointers.RightButton;
            }

            if (pointerPoint.Properties.IsMiddleButtonPressed)
            {
                pointers |= Pointers.MiddleButton;
            }

            if (pointerPoint.Properties.IsXButton1Pressed)
            {
                pointers |= Pointers.XButton1;
            }

            if (pointerPoint.Properties.IsXButton2Pressed)
            {
                pointers |= Pointers.XButton2;
            }

            SKPoint screenPosition = SKPoint(pointerPoint.Position);

            return new(screenPosition,
                       screenPosition.ToWorld(this),
                       GetModifiers(e.KeyModifiers),
                       pointers);
        }

        PointerWheelEventArgs PointerWheelEventArgs(PointerRoutedEventArgs e)
        {
            PointerPoint pointerPoint = e.GetCurrentPoint(this);

            SKPoint screenPosition = SKPoint(pointerPoint.Position);

            return new(screenPosition,
                       screenPosition.ToWorld(this),
                       pointerPoint.Properties.MouseWheelDelta,
                       GetModifiers(e.KeyModifiers));
        }

        static Modifiers GetModifiers(VirtualKeyModifiers keyModifiers)
        {
            Modifiers result = Modifiers.None;

            if (keyModifiers.HasFlag(VirtualKeyModifiers.Control))
            {
                result |= Modifiers.Control;
            }

            if (keyModifiers.HasFlag(VirtualKeyModifiers.Menu))
            {
                result |= Modifiers.Menu;
            }

            if (keyModifiers.HasFlag(VirtualKeyModifiers.Shift))
            {
                result |= Modifiers.Shift;
            }

            if (keyModifiers.HasFlag(VirtualKeyModifiers.Windows))
            {
                result |= Modifiers.Windows;
            }

            return result;
        }
    }

    SKSize IBlueprintEditor.Extent => new((float)ActualWidth, (float)ActualHeight);

    public float X
    {
        get => (float)GetValue(XProperty);
        set => SetValue(XProperty, value);
    }

    public float Y
    {
        get => (float)GetValue(YProperty);
        set => SetValue(YProperty, value);
    }

    public float Zoom
    {
        get => (float)GetValue(ZoomProperty);
        set => SetValue(ZoomProperty, value);
    }

    public FontFamily? FontFamily
    {
        get => (FontFamily?)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public IBlueprintTheme? Theme
    {
        get => (IBlueprintTheme)GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    public IEnumerable<Element>? Elements
    {
        get => (IEnumerable<Element>)GetValue(ElementsProperty);
        set => SetValue(ElementsProperty, value);
    }

    string IBlueprintEditor.FontFamily => FontFamily?.Source ?? FontFamily.XamlAutoFontFamily.Source;

    IBlueprintTheme IBlueprintEditor.Theme => Theme ?? DefaultBlueprintTheme.Instance;

    IEnumerable<Element> IBlueprintEditor.Elements => Elements ??= [];

    void IBlueprintEditor.Invalidate()
    {
        isInvalidateScheduled = true;
    }

    SKTypeface IBlueprintEditor.ResolveTypeface(string fontFamily, SKFontStyleWeight weight)
    {
        if (Uri.TryCreate(fontFamily, UriKind.Absolute, out Uri? uri) && uri.Scheme.Equals("ms-appx", StringComparison.OrdinalIgnoreCase))
        {
            StorageFile file = StorageFile.GetFileFromApplicationUriAsync(uri).GetResults();

            using Stream stream = file.OpenStreamForReadAsync().GetAwaiter().GetResult();

            return SKTypeface.FromStream(stream);
        }

        return SKTypeface.FromFamilyName(fontFamily, new(weight, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright));
    }

    private void Rendering(object? sender, object e)
    {
        if (isInvalidateScheduled)
        {
            Invalidate();

            isInvalidateScheduled = false;
        }
    }
}
