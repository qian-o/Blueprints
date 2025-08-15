using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using SkiaSharp;
using SkiaSharp.Views.Windows;

namespace Blueprints.WinUI;

public sealed partial class BlueprintEditor : SKXamlCanvas, IBlueprintEditor
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
                                                                                          new PropertyMetadata(new DefaultBlueprintTheme()));

    public static readonly DependencyProperty ElementsProperty = DependencyProperty.Register(nameof(Elements),
                                                                                             typeof(IEnumerable<Element>),
                                                                                             typeof(BlueprintEditor),
                                                                                             new PropertyMetadata(null));

    public BlueprintEditor()
    {
        BlueprintRenderer renderer = new(this);
        BlueprintController controller = new(this);

        PaintSurface += (_, e) => renderer.Render(e.Surface.Canvas, (float)Dpi);

        PointerPressed += (_, e) => controller.PointerPressed(PointerEventArgs(e.GetCurrentPoint(this)));

        PointerMoved += (_, e) => controller.PointerMoved(PointerEventArgs(e.GetCurrentPoint(this)));

        PointerReleased += (_, e) => controller.PointerReleased(PointerEventArgs(e.GetCurrentPoint(this)));

        PointerWheelChanged += (_, e) => controller.PointerWheelChanged(PointerWheelEventArgs(e.GetCurrentPoint(this)));

        PointerEventArgs PointerEventArgs(PointerPoint pointerPoint)
        {
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

            SKPoint screenPosition = pointerPoint.Position.ToSKPoint();

            return new(screenPosition, screenPosition.ToWorld(this), pointers);
        }

        PointerWheelEventArgs PointerWheelEventArgs(PointerPoint pointerPoint)
        {
            SKPoint screenPosition = pointerPoint.Position.ToSKPoint();

            return new(screenPosition, screenPosition.ToWorld(this), pointerPoint.Properties.MouseWheelDelta);
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

    IBlueprintTheme IBlueprintEditor.Theme => Theme ?? new DefaultBlueprintTheme();

    IEnumerable<Element> IBlueprintEditor.Elements => Elements ?? [];

    byte[] IBlueprintEditor.FontFileResolver(string fontFamily)
    {
        return [];
    }
}
