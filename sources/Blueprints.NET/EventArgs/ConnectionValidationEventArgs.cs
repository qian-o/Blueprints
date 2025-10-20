namespace Blueprints.NET;

public class ConnectionValidationEventArgs(Pin source, Pin target) : EventArgs
{
    public Pin Source { get; } = source;

    public Pin Target { get; } = target;

    public bool Cancel { get; set; }
}
