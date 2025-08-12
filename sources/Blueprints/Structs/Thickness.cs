namespace Blueprints;

public readonly struct Thickness(float left, float top, float right, float bottom)
{
    public float Left { get; } = left;

    public float Top { get; } = top;

    public float Right { get; } = right;

    public float Bottom { get; } = bottom;

    public bool IsZero { get; } = left == 0.0f && top == 0.0f && right == 0.0f && bottom == 0.0f;

    public bool IsUniform { get; } = left == right && top == bottom && left == top;

    public static implicit operator Thickness(float uniform)
    {
        return new Thickness(uniform, uniform, uniform, uniform);
    }
}
