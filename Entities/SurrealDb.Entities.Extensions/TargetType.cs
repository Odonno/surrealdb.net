namespace SurrealDb.Entities.Extensions;

internal readonly struct TargetType
{
    public string Namespace { get; }
    public string ClassName { get; }

    public TargetType(string @namespace, string className)
    {
        Namespace = @namespace;
        ClassName = className;
    }
}
