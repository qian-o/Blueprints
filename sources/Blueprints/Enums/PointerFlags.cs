namespace Blueprints;

[Flags]
public enum PointerFlags
{
    None = 0,

    LeftButton = 1 << 0,

    RightButton = 1 << 1,

    MiddleButton = 1 << 2,

    XButton1 = 1 << 3,

    XButton2 = 1 << 4
}
