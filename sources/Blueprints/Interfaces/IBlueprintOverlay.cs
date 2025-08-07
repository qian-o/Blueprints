namespace Blueprints;

public interface IBlueprintOverlay
{
    void Render(object overlay, float x, float y);

    void Destroy(object overlay);
}
