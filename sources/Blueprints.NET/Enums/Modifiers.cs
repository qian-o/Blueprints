namespace Blueprints.NET;

[Flags]
public enum Modifiers
{
    None = 0,

    Control = 1 << 0,

    Menu = 1 << 1,

    Shift = 1 << 2,

    Windows = 1 << 3
}
