using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FriendlyEnum;

public class FriendlyEnumSyntaxReceiver : ISyntaxReceiver
{
    public Dictionary<EnumDeclarationSyntax, List<EnumMemberDeclarationSyntax>> Enums { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not EnumDeclarationSyntax enumDeclaration)
            return;

        if (!HasFriendlyEnumAttribute(enumDeclaration))
            return;

        Enums.Add(enumDeclaration, enumDeclaration.Members.ToList());
    }

    private bool HasFriendlyEnumAttribute(EnumDeclarationSyntax syntax)
    {
        return syntax.AttributeLists
            .SelectMany(x => x.Attributes)
            .Any(SyntaxExtensions.IsFriendlyEnumAttribute);
    }
}