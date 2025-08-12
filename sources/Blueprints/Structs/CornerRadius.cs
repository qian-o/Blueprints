namespace Blueprints;

public readonly struct CornerRadius(float topLeft, float topRight, float bottomLeft, float bottomRight)
{
    public float TopLeft { get; } = topLeft;

    public float TopRight { get; } = topRight;

    public float BottomLeft { get; } = bottomLeft;

    public float BottomRight { get; } = bottomRight;

    public bool IsZero { get; } = topLeft == 0.0f && topRight == 0.0f && bottomLeft == 0.0f && bottomRight == 0.0f;

    public bool IsUniform { get; } = topLeft == topRight && topLeft == bottomLeft && topLeft == bottomRight;

    public static implicit operator CornerRadius(float radius)
    {
        return new CornerRadius(radius, radius, radius, radius);
    }
}
