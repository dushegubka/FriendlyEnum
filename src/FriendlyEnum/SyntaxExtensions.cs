using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
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
        var tree = CSharpSyntaxTree.ParseText(sourceText.ToString());
        var root = tree.GetRoot();

        var workspace = new AdhocWorkspace();
        var options = workspace.Options;
        options = options.WithChangedOption(FormattingOptions.UseTabs, LanguageNames.CSharp, false);
        options = options.WithChangedOption(FormattingOptions.TabSize, LanguageNames.CSharp, 4);

        var formattedNode = Formatter.Format(root, workspace, options);

        return SourceText.From(formattedNode.ToFullString(), Encoding.UTF8);
    }
    

    private static IEnumerable<AttributeSyntax> GetAttributes(AttributeListSyntax attributeList)
    {
        return attributeList.Attributes;
    }

    private static bool IsFriendlyNameAttribute(AttributeSyntax attribute)
    {
        return attribute.Name.ToString() == "FriendlyName";
    }

    private static string GetExpressionText(AttributeArgumentSyntax argument)
    {
        return argument.Expression.ToString();
    }
}