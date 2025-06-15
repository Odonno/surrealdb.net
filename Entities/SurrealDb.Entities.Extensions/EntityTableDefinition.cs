namespace SurrealDb.Entities.Extensions;

internal readonly struct EntityTableDefinition
{
    public string TableName { get; }
    public string TypeName { get; }
    public string TypeNamespace { get; }

    public string EntityPluralName => $"{TypeName}s";
    public string TypeFullName => $"{TypeNamespace}.{TypeName}";

    public EntityTableDefinition(string tableName, string typeName, string typeNamespace)
    {
        TableName = tableName;
        TypeName = typeName;
        TypeNamespace = typeNamespace;
    }

    public string ToExtensionPropertyTemplate(int indent = 2)
    {
        var indentStr = Enumerable.Repeat("    ", indent);

        return $"{string.Concat(indentStr)}public Task<IEnumerable<{TypeFullName}>> {EntityPluralName} => source.Select<{TypeFullName}>(\"{TableName}\");";
    }
}
