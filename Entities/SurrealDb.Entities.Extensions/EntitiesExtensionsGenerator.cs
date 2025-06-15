using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SurrealDb.Entities.Extensions;

[Generator]
public class EntitiesExtensionsGenerator : IIncrementalGenerator
{
    private const string CustomAttributesNamespace = "SurrealDb.Net";

    private readonly TargetType _defaultTargetType = new("SurrealDb.Net", "ISurrealDbClient");

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Add the marker attribute to the compilation.
        context.RegisterPostInitializationOutput(ctx =>
            ctx.AddSource(
                "EntitiesGeneratorRecordsFromNamespaceAttribute.g.cs",
                SourceText.From(
                    AttributesSourceCode.EntitiesGeneratorRecordsFromNamespaceAttribute,
                    Encoding.UTF8
                )
            )
        );

        // Filter classes annotated with the [EntitiesGeneratorRecordsFromNamespace] attribute.
        // Only filtered Syntax Nodes can trigger code generation.
        var provider = context
            .SyntaxProvider.CreateSyntaxProvider(
                (s, _) => s is ClassDeclarationSyntax,
                (ctx, _) => GetClassDeclarationForSourceGen(ctx)
            )
            .Where(t => t.Type != ClassDeclarationSyntaxType.Unknown)
            .Select((t, _) => t.ClassDeclarationSyntax);

        // Generate the source code.
        context.RegisterSourceOutput(
            context.CompilationProvider.Combine(provider.Collect()),
            ((ctx, t) => GenerateCode(ctx, t.Left, t.Right))
        );
    }

    private static ClassDeclarationForSourceGen GetClassDeclarationForSourceGen(
        GeneratorSyntaxContext context
    )
    {
        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

        if (IsRecordClass(context.SemanticModel, classDeclarationSyntax))
        {
            return new ClassDeclarationForSourceGen(
                classDeclarationSyntax,
                ClassDeclarationSyntaxType.Record
            );
        }
        if (IsGeneratorWithAttributesClass(context.SemanticModel, classDeclarationSyntax))
        {
            return new ClassDeclarationForSourceGen(
                classDeclarationSyntax,
                ClassDeclarationSyntaxType.GeneratorWithAttributesClass
            );
        }

        return new ClassDeclarationForSourceGen(
            classDeclarationSyntax,
            ClassDeclarationSyntaxType.Unknown
        );
    }

    private static bool IsRecordClass(
        SemanticModel semanticModel,
        ClassDeclarationSyntax classDeclarationSyntax
    )
    {
        return InheritsIRecord(semanticModel, classDeclarationSyntax);
    }

    private static bool InheritsIRecord(
        SemanticModel semanticModel,
        ClassDeclarationSyntax classDeclarationSyntax
    )
    {
        if (classDeclarationSyntax.BaseList is null)
        {
            return false;
        }

        foreach (var baseTypeSyntax in classDeclarationSyntax.BaseList.Types)
        {
            var baseSymbol = semanticModel.GetSymbolInfo(baseTypeSyntax.Type).Symbol;
            if (baseSymbol is null)
            {
                continue;
            }

            string baseName = baseSymbol.ToDisplayString();

            const string recordClassesNamespace = "SurrealDb.Net.Models";
            if (
                string.Equals(
                    baseName,
                    $"{recordClassesNamespace}.Record",
                    StringComparison.Ordinal
                )
                || string.Equals(
                    baseName,
                    $"{recordClassesNamespace}.IRecord",
                    StringComparison.Ordinal
                )
            )
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsGeneratorWithAttributesClass(
        SemanticModel semanticModel,
        ClassDeclarationSyntax classDeclarationSyntax
    )
    {
        // Go through all attributes of the class
        foreach (var attributeListSyntax in classDeclarationSyntax.AttributeLists)
        foreach (var attributeSyntax in attributeListSyntax.Attributes)
        {
            var attributeSymbol = GetAttributeSymbol(semanticModel, attributeSyntax);
            if (attributeSymbol is null)
            {
                continue;
            }

            string attributeName = attributeSymbol.ContainingType.ToDisplayString();

            // Check the full name of the [EntitiesGeneratorRecordsFromNamespace] attribute
            const string expectedAttributeName = "EntitiesGeneratorRecordsFromNamespaceAttribute";
            if (
                string.Equals(
                    attributeName,
                    $"{CustomAttributesNamespace}.{expectedAttributeName}",
                    StringComparison.Ordinal
                )
            )
            {
                return true;
            }
        }

        return false;
    }

    private static ISymbol? GetAttributeSymbol(
        SemanticModel semanticModel,
        AttributeSyntax attributeSyntax
    )
    {
        var symbolInfo = semanticModel.GetSymbolInfo(attributeSyntax.Name);

        var symbol = symbolInfo.Symbol;
        if (symbol is not null)
        {
            return symbol;
        }

        var candidateSymbols = symbolInfo.CandidateSymbols;
        if (candidateSymbols.IsEmpty || candidateSymbols.Length > 1)
        {
            return null;
        }

        return symbolInfo.CandidateSymbols.Single();
    }

    private void GenerateCode(
        SourceProductionContext context,
        Compilation compilation,
        ImmutableArray<ClassDeclarationSyntax> classDeclarations
    )
    {
        // Go through all filtered class declarations.
        foreach (var classDeclarationSyntax in classDeclarations)
        {
            // We need to get semantic model of the class to retrieve metadata.
            var semanticModel = compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);

            // Symbols allow us to get the compile-time information.
            if (
                semanticModel.GetDeclaredSymbol(classDeclarationSyntax)
                is not INamedTypeSymbol classSymbol
            )
            {
                continue;
            }

            if (!IsGeneratorWithAttributesClass(semanticModel, classDeclarationSyntax))
            {
                continue;
            }

            string namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
            namespaceName = namespaceName == "<global namespace>" ? string.Empty : namespaceName;

            // 'Identifier' means the token of the node. Get class name from the syntax node.
            var className = classDeclarationSyntax.Identifier.Text;

            var fromNamespaces = GetFromNamespacesFromAttribute(
                semanticModel,
                classDeclarationSyntax
            );

            var entityTableDefinitions = GetEntityTableDefinitions(
                compilation,
                classDeclarations,
                fromNamespaces.ToArray()
            );

            var propertyExtensionTemplates = entityTableDefinitions.OrderBy(etd =>
                etd.EntityPluralName
            );

            // Build up the source code
            string code = $$"""
                // <auto-generated/>

                namespace {{_defaultTargetType.Namespace}};

                public static class Entities{{_defaultTargetType.ClassName}}Extensions
                {
                //#if NET10_0_OR_GREATER
                    extension({{_defaultTargetType.ClassName}} source)
                    {
                {{string.Join(
                    "\n",
                    propertyExtensionTemplates.Select(etd => etd.ToExtensionPropertyTemplate())
                )}}
                    }
                //#endif
                }
                
                """;

            string sourceFileName = string.IsNullOrWhiteSpace(namespaceName)
                ? className
                : $"{namespaceName}.{className}";

            context.AddSource($"{sourceFileName}.g.cs", SourceText.From(code, Encoding.UTF8));
        }
    }

    private static IEnumerable<string> GetFromNamespacesFromAttribute(
        SemanticModel semanticModel,
        ClassDeclarationSyntax classDeclarationSyntax
    )
    {
        foreach (var attributeListSyntax in classDeclarationSyntax.AttributeLists)
        foreach (var attributeSyntax in attributeListSyntax.Attributes)
        {
            var attributeSymbol = GetAttributeSymbol(semanticModel, attributeSyntax);
            if (attributeSymbol is null)
            {
                continue;
            }

            string attributeName = attributeSymbol.ContainingType.ToDisplayString();

            const string expectedAttributeName = "EntitiesGeneratorRecordsFromNamespaceAttribute";
            if (
                !string.Equals(
                    attributeName,
                    $"{CustomAttributesNamespace}.{expectedAttributeName}",
                    StringComparison.Ordinal
                )
            )
            {
                continue;
            }

            var arg = attributeSyntax
                .ArgumentList?.Arguments[0]
                ?.Expression?.ChildTokens()
                ?.SingleOrDefault()
                .ValueText;
            if (string.IsNullOrEmpty(arg))
            {
                continue;
            }

            yield return arg!;
        }
    }

    private static IEnumerable<EntityTableDefinition> GetEntityTableDefinitions(
        Compilation compilation,
        ImmutableArray<ClassDeclarationSyntax> classDeclarations,
        string[] fromNamespaces
    )
    {
        foreach (var classDeclarationSyntax in classDeclarations)
        {
            // We need to get semantic model of the class to retrieve metadata.
            var semanticModel = compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);

            // Symbols allow us to get the compile-time information.
            if (
                semanticModel.GetDeclaredSymbol(classDeclarationSyntax)
                is not INamedTypeSymbol classSymbol
            )
            {
                continue;
            }

            if (!IsRecordClass(semanticModel, classDeclarationSyntax))
            {
                continue;
            }

            var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();

            if (
                !fromNamespaces.Any(n =>
                    string.Equals(namespaceName, n, StringComparison.Ordinal)
                    || namespaceName.StartsWith($"{n}.")
                )
            )
            {
                continue;
            }

            // 'Identifier' means the token of the node. Get class name from the syntax node.
            var className = classDeclarationSyntax.Identifier.Text;

            var tableNameFromAttribute = GetTableNameFromAttribute(
                semanticModel,
                classDeclarationSyntax
            );

            var value = new EntityTableDefinition(
                tableNameFromAttribute ?? className,
                className,
                namespaceName
            );
            yield return value;
        }
    }

    private static string? GetTableNameFromAttribute(
        SemanticModel semanticModel,
        ClassDeclarationSyntax classDeclarationSyntax
    )
    {
        const string tableAttributeFullname =
            "System.ComponentModel.DataAnnotations.Schema.TableAttribute";

        foreach (var attributeListSyntax in classDeclarationSyntax.AttributeLists)
        foreach (var attributeSyntax in attributeListSyntax.Attributes)
        {
            var attributeSymbol = GetAttributeSymbol(semanticModel, attributeSyntax);
            if (attributeSymbol is null)
            {
                continue;
            }

            string attributeName = attributeSymbol.ContainingType.ToDisplayString();

            if (!string.Equals(attributeName, tableAttributeFullname))
            {
                continue;
            }

            var arg = attributeSyntax
                .ArgumentList?.Arguments[0]
                ?.Expression?.ChildTokens()
                ?.SingleOrDefault()
                .ValueText;
            if (string.IsNullOrEmpty(arg))
            {
                continue;
            }

            return arg;
        }

        return null;
    }
}
