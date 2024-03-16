using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace FriendlyEnum;

public static class SyntaxExtensions
{
    public static string? GetFriendlyNameAttributeValue(this EnumMemberDeclarationSyntax enumMember)
    {
        return enumMember
            .AttributeLists
            .SelectMany(GetAttributes)
            .FirstOrDefault(IsFriendlyNameAttribute)
            ?.ArgumentList?.Arguments
            .Select(GetExpressionText)
            .FirstOrDefault();
    }

    public static SourceText ReformatAndCleanup(this SourceText sourceText)
    {
        return CSharpSyntaxTree.ParseText(sourceText)
            .GetRoot()
            .NormalizeWhitespace()
            .GetText(Encoding.UTF8);
    }

    private static IEnumerable<AttributeSyntax> GetAttributes(AttributeListSyntax attributeList)
    {
        return attributeList.Attributes;
    }

    public static bool IsFriendlyNameAttribute(AttributeSyntax attribute)
    {
        return attribute.Name.ToString() == GeneratorConstants.FriendlyNameAttributeName
            .Replace(nameof(Attribute), string.Empty);
    }
    
    public static bool IsFriendlyEnumAttribute(AttributeSyntax attribute)
    {
        return attribute.Name.ToString() == GeneratorConstants.FriendlyEnumAttributeName
            .Replace(nameof(Attribute), string.Empty);
    }

    private static string GetExpressionText(AttributeArgumentSyntax argument)
    {
        return argument.Expression.ToString();
    }
}