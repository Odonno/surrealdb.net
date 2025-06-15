namespace SurrealDb.Entities.Extensions;

internal static class AttributesSourceCode
{
    internal const string EntitiesGeneratorRecordsFromNamespaceAttribute = """
        namespace SurrealDb.Net;

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class EntitiesGeneratorRecordsFromNamespaceAttribute : Attribute
        {
            public string Namespace { get; set; }

            public EntitiesGeneratorRecordsFromNamespaceAttribute(string namespaceName)
            {
                Namespace = namespaceName;
            }
        }

        """;
}
