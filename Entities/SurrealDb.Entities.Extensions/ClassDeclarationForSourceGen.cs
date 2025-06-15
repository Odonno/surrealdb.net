using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SurrealDb.Entities.Extensions;

internal readonly struct ClassDeclarationForSourceGen
{
    public ClassDeclarationSyntax ClassDeclarationSyntax { get; }
    public ClassDeclarationSyntaxType Type { get; }

    public ClassDeclarationForSourceGen(
        ClassDeclarationSyntax classDeclarationSyntax,
        ClassDeclarationSyntaxType type
    )
    {
        ClassDeclarationSyntax = classDeclarationSyntax;
        Type = type;
    }
}
