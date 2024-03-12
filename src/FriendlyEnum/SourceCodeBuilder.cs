using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace FriendlyEnum;

public static class SourceCodeBuilder
{
    public static SourceText BuildExtensionClassSourceCode(
        string? extensionClassName,
        Dictionary<EnumDeclarationSyntax, List<EnumMemberDeclarationSyntax>> enums)
    {
        var builder = new StringBuilder();

        builder.AppendLine($"public static class {extensionClassName}");
        builder.AppendLine("{");
        
        foreach (var item in enums)
        {
            builder.AppendLine(BuildExtensionMethodSourceCode(item.Key, item.Value));
        }
        
        builder.AppendLine("}");
        
        return SourceText.From(builder.ToString(), Encoding.UTF8);
    }

    private static string BuildExtensionMethodSourceCode(
        EnumDeclarationSyntax enumDeclaration,
        List<EnumMemberDeclarationSyntax> enumMembers)
    {
        var builder = new StringBuilder();
        var enumName = enumDeclaration.Identifier.Text;
        
        builder.AppendLine($"public static string ToFriendlyName(this {enumName} value)");
        builder.AppendLine("{");
        builder.AppendLine("return value switch");
        builder.AppendLine("{");
        
        foreach (var enumMember in enumMembers)
        {
            var currentMemberName = enumMember.Identifier.Text;
            var friendlyName = enumMember.GetFriendlyNameAttributeValue();
            
            builder.AppendLine(string.IsNullOrEmpty(friendlyName)
                // Example: Enum.One => nameof(Enum.One)
                ? $"{enumName}.{currentMemberName} => nameof({enumName}.{currentMemberName}),"
                // Example: Enum.One => "First"
                : $"{enumName}.{currentMemberName} => {friendlyName},");
        }
        
        builder.AppendLine("_ => throw new ArgumentException(nameof(value))");
        builder.AppendLine("};");
        builder.AppendLine("}");
        
        return builder.ToString();
    }
}